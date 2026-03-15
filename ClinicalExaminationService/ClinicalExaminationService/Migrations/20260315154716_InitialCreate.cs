using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClinicalExaminationService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignmentSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NurseId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClinicRoom = table.Column<string>(type: "text", nullable: false),
                    ShiftDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ShiftType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Icd10Catalogs",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icd10Catalogs", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "VisitRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceptionistId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    QueueNumber = table.Column<int>(type: "integer", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsPriority = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "follow_up_appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VisitId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowUpDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClinicalNotes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow_up_appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_follow_up_appointments_VisitRecords_VisitId",
                        column: x => x.VisitId,
                        principalTable: "VisitRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalExaminations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VisitId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Symptoms = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Diagnosis = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ExaminedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalExaminations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalExaminations_VisitRecords_VisitId",
                        column: x => x.VisitId,
                        principalTable: "VisitRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VisitId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PaymentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DispenseStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prescriptions_VisitRecords_VisitId",
                        column: x => x.VisitId,
                        principalTable: "VisitRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VisitId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    ServiceName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UnitPriceAtOrder = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_service_orders_VisitRecords_VisitId",
                        column: x => x.VisitId,
                        principalTable: "VisitRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VitalSigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VisitId = table.Column<Guid>(type: "uuid", nullable: false),
                    NurseId = table.Column<Guid>(type: "uuid", nullable: true),
                    Pulse = table.Column<int>(type: "integer", nullable: false),
                    Temperature = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: false),
                    BloodPressure = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VitalSigns_VisitRecords_VisitId",
                        column: x => x.VisitId,
                        principalTable: "VisitRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExaminationIcd10s",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExaminationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Icd10Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationIcd10s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExaminationIcd10s_Icd10Catalogs_Icd10Code",
                        column: x => x.Icd10Code,
                        principalTable: "Icd10Catalogs",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExaminationIcd10s_MedicalExaminations_ExaminationId",
                        column: x => x.ExaminationId,
                        principalTable: "MedicalExaminations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prescription_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PrescriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicationName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Dosage = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prescription_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prescription_items_prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Icd10Catalogs",
                columns: new[] { "Code", "Description" },
                values: new object[,]
                {
                    { "A00", "Bệnh tả" },
                    { "A01", "Bệnh thương hàn và phó thương hàn" },
                    { "A02", "Nhiễm trùng do Salmonella khác" },
                    { "A09", "Tiêu chảy và viêm dạ dày ruột do nhiễm khuẩn" },
                    { "A15", "Lao phổi, xác nhận bằng nuôi cấy vi khuẩn" },
                    { "A16", "Lao phổi, không xác nhận bằng vi khuẩn học" },
                    { "B01", "Thủy đậu" },
                    { "B15", "Viêm gan siêu vi A" },
                    { "B16", "Viêm gan siêu vi B cấp tính" },
                    { "B34", "Nhiễm virus, chưa xác định vị trí" },
                    { "E10", "Đái tháo đường type 1" },
                    { "E11", "Đái tháo đường type 2" },
                    { "E14", "Đái tháo đường, không đặc hiệu" },
                    { "F32", "Giai đoạn trầm cảm" },
                    { "G40", "Động kinh" },
                    { "I10", "Tăng huyết áp nguyên phát (Huyết áp cao)" },
                    { "I20", "Đau thắt ngực" },
                    { "I21", "Nhồi máu cơ tim cấp tính" },
                    { "I50", "Suy tim" },
                    { "J00", "Viêm mũi họng cấp tính (Cảm lạnh thông thường)" },
                    { "J06", "Nhiễm trùng đường hô hấp trên cấp tính, không đặc hiệu" },
                    { "J18", "Viêm phổi, vi sinh vật không đặc hiệu" },
                    { "J45", "Hen phế quản (Asthma)" },
                    { "K21", "Bệnh trào ngược dạ dày thực quản (GERD)" },
                    { "K29", "Viêm dạ dày và tá tràng" },
                    { "K35", "Viêm ruột thừa cấp tính" },
                    { "M54", "Đau lưng (Dorsalgia)" },
                    { "N39", "Rối loạn khác của đường tiết niệu" },
                    { "R50", "Sốt không rõ nguyên nhân" },
                    { "Z00", "Khám sức khỏe tổng quát" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationIcd10s_ExaminationId_Icd10Code",
                table: "ExaminationIcd10s",
                columns: new[] { "ExaminationId", "Icd10Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationIcd10s_Icd10Code",
                table: "ExaminationIcd10s",
                column: "Icd10Code");

            migrationBuilder.CreateIndex(
                name: "IX_follow_up_appointments_VisitId",
                table: "follow_up_appointments",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalExaminations_VisitId",
                table: "MedicalExaminations",
                column: "VisitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prescription_items_PrescriptionId",
                table: "prescription_items",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_prescriptions_VisitId",
                table: "prescriptions",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_VisitId",
                table: "service_orders",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSigns_VisitId",
                table: "VitalSigns",
                column: "VisitId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentSchedules");

            migrationBuilder.DropTable(
                name: "ExaminationIcd10s");

            migrationBuilder.DropTable(
                name: "follow_up_appointments");

            migrationBuilder.DropTable(
                name: "prescription_items");

            migrationBuilder.DropTable(
                name: "service_orders");

            migrationBuilder.DropTable(
                name: "VitalSigns");

            migrationBuilder.DropTable(
                name: "Icd10Catalogs");

            migrationBuilder.DropTable(
                name: "MedicalExaminations");

            migrationBuilder.DropTable(
                name: "prescriptions");

            migrationBuilder.DropTable(
                name: "VisitRecords");
        }
    }
}
