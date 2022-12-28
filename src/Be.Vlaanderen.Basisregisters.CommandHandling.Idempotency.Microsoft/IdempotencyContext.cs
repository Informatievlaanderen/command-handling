namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft
{
    using global::Microsoft.EntityFrameworkCore;
    using global::Microsoft.EntityFrameworkCore.Design;

    public class IdempotencyContext : DbContext
    {
        private readonly IdempotencyTableInfo _idempotencyTableInfo;

        public DbSet<ProcessedCommand> ProcessedCommands { get; set; }

        // This needs to be DbContextOptions<T> for Autofac!
        public IdempotencyContext(
            DbContextOptions<IdempotencyContext> options,
            IdempotencyTableInfo idempotencyTableInfo) : base(options)
        {
            _idempotencyTableInfo = idempotencyTableInfo;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var processedCommand = modelBuilder.Entity<ProcessedCommand>();

            processedCommand.ToTable(_idempotencyTableInfo.TableName, _idempotencyTableInfo.Schema)
                .HasKey(p => new { p.CommandId, p.CommandContentHash })
                .IsClustered(false);

            processedCommand.Property(p => p.CommandContentHash).HasMaxLength(128); // SHA-512 Hex

            processedCommand.Property(p => p.DateProcessed);

            processedCommand.HasIndex(p => p.DateProcessed).IsClustered();
        }
    }

    public class IdempotencyContextFactory : IDesignTimeDbContextFactory<IdempotencyContext>
    {
        public IdempotencyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdempotencyContext>();
            var tableInfo = new IdempotencyTableInfo("dbo");

            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory.IdempotencyContext;Trusted_Connection=True;");

            return new IdempotencyContext(optionsBuilder.Options, tableInfo);
        }
    }
}
