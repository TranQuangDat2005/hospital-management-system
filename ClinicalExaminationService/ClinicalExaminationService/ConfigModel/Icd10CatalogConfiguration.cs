using ClinicalExaminationService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicalExaminationService.ConfigModel
{
    public class Icd10CatalogConfiguration : IEntityTypeConfiguration<Icd10Catalog>
    {
        public void Configure(EntityTypeBuilder<Icd10Catalog> builder)
        {
            builder.HasKey(c => c.Code);

            builder.Property(c => c.Code)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(c => c.Description)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.HasData(
                new Icd10Catalog { Code = "A00", Description = "Bệnh tả" },
                new Icd10Catalog { Code = "A01", Description = "Bệnh thương hàn và phó thương hàn" },
                new Icd10Catalog { Code = "A02", Description = "Nhiễm trùng do Salmonella khác" },
                new Icd10Catalog { Code = "A09", Description = "Tiêu chảy và viêm dạ dày ruột do nhiễm khuẩn" },
                new Icd10Catalog { Code = "A15", Description = "Lao phổi, xác nhận bằng nuôi cấy vi khuẩn" },
                new Icd10Catalog { Code = "A16", Description = "Lao phổi, không xác nhận bằng vi khuẩn học" },
                new Icd10Catalog { Code = "B01", Description = "Thủy đậu" },
                new Icd10Catalog { Code = "B15", Description = "Viêm gan siêu vi A" },
                new Icd10Catalog { Code = "B16", Description = "Viêm gan siêu vi B cấp tính" },
                new Icd10Catalog { Code = "B34", Description = "Nhiễm virus, chưa xác định vị trí" },
                new Icd10Catalog { Code = "E10", Description = "Đái tháo đường type 1" },
                new Icd10Catalog { Code = "E11", Description = "Đái tháo đường type 2" },
                new Icd10Catalog { Code = "E14", Description = "Đái tháo đường, không đặc hiệu" },
                new Icd10Catalog { Code = "F32", Description = "Giai đoạn trầm cảm" },
                new Icd10Catalog { Code = "G40", Description = "Động kinh" },
                new Icd10Catalog { Code = "I10", Description = "Tăng huyết áp nguyên phát (Huyết áp cao)" },
                new Icd10Catalog { Code = "I20", Description = "Đau thắt ngực" },
                new Icd10Catalog { Code = "I21", Description = "Nhồi máu cơ tim cấp tính" },
                new Icd10Catalog { Code = "I50", Description = "Suy tim" },
                new Icd10Catalog { Code = "J00", Description = "Viêm mũi họng cấp tính (Cảm lạnh thông thường)" },
                new Icd10Catalog { Code = "J06", Description = "Nhiễm trùng đường hô hấp trên cấp tính, không đặc hiệu" },
                new Icd10Catalog { Code = "J18", Description = "Viêm phổi, vi sinh vật không đặc hiệu" },
                new Icd10Catalog { Code = "J45", Description = "Hen phế quản (Asthma)" },
                new Icd10Catalog { Code = "K21", Description = "Bệnh trào ngược dạ dày thực quản (GERD)" },
                new Icd10Catalog { Code = "K29", Description = "Viêm dạ dày và tá tràng" },
                new Icd10Catalog { Code = "K35", Description = "Viêm ruột thừa cấp tính" },
                new Icd10Catalog { Code = "M54", Description = "Đau lưng (Dorsalgia)" },
                new Icd10Catalog { Code = "N39", Description = "Rối loạn khác của đường tiết niệu" },
                new Icd10Catalog { Code = "R50", Description = "Sốt không rõ nguyên nhân" },
                new Icd10Catalog { Code = "Z00", Description = "Khám sức khỏe tổng quát" }
            );
        }
    }
}
