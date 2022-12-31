// <auto-generated />
using Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Microsoft;
using global::Microsoft.EntityFrameworkCore;
using global::Microsoft.EntityFrameworkCore.Infrastructure;
using global::Microsoft.EntityFrameworkCore.Metadata;
using global::Microsoft.EntityFrameworkCore.Migrations;
using global::Microsoft.EntityFrameworkCore.Storage;
using global::Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Migrations
{
    [DbContext(typeof(IdempotencyContext))]
    [Migration("20180309133901_InitialIdempotencyMigration")]
    partial class InitialIdempotencyMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.ProcessedCommand", b =>
                {
                    b.Property<Guid>("CommandId");

                    b.Property<string>("CommandContentHash")
                        .HasMaxLength(128);

                    b.Property<DateTimeOffset>("DateProcessed");

                    b.HasKey("CommandId", "CommandContentHash")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("DateProcessed")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable(MigrationsHelper.TableInfo.TableName, MigrationsHelper.TableInfo.Schema);
                });
#pragma warning restore 612, 618
        }
    }
}