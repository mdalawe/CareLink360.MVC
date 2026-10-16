using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareLink360.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNinAndMedical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "NinVerifications");

            migrationBuilder.DropColumn(
                name: "IsBackgroundChecked",
                table: "Caregivers");

            migrationBuilder.DropColumn(
                name: "IsMedicallyCleared",
                table: "Caregivers");

            migrationBuilder.DropColumn(
                name: "IsNinVerified",
                table: "Caregivers");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_JobTypeId",
                table: "JobRequests",
                column: "JobTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRequests_JobTypes_JobTypeId",
                table: "JobRequests",
                column: "JobTypeId",
                principalTable: "JobTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRequests_JobTypes_JobTypeId",
                table: "JobRequests");

            migrationBuilder.DropIndex(
                name: "IX_JobRequests_JobTypeId",
                table: "JobRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsBackgroundChecked",
                table: "Caregivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMedicallyCleared",
                table: "Caregivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNinVerified",
                table: "Caregivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaregiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClearanceStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastCheckupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VaccinationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NinVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    Nin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinVerifications", x => x.Id);
                });
        }
    }
}
