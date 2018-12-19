namespace Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitialIdempotencyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: MigrationsHelper.TableInfo.Schema);

            migrationBuilder.CreateTable(
                name: MigrationsHelper.TableInfo.TableName,
                schema: MigrationsHelper.TableInfo.Schema,
                columns: table => new
                {
                    CommandId = table.Column<Guid>(nullable: false),
                    CommandContentHash = table.Column<string>(maxLength: 128, nullable: false),
                    DateProcessed = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedCommands", x => new { x.CommandId, x.CommandContentHash })
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedCommands_DateProcessed",
                schema: MigrationsHelper.TableInfo.Schema,
                table: MigrationsHelper.TableInfo.TableName,
                column: "DateProcessed")
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: MigrationsHelper.TableInfo.TableName,
                schema: MigrationsHelper.TableInfo.Schema);
        }
    }
}
