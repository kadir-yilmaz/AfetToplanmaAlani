using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.DAL;
using AfetToplanmaAlani.EL.Dtos;
using AfetToplanmaAlani.EL.Models;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;

namespace AfetToplanmaAlani.BLL.Concrete
{
    public class WorkGroupStaffService : IWorkGroupStaffService
    {
        private readonly AppDbContext _context;
        private readonly IExcelService _excelService;

        public WorkGroupStaffService(AppDbContext context, IExcelService excelService)
        {
            _context = context;
            _excelService = excelService;
        }

        public async Task CreateStaffAsync(WorkGroupStaff staff)
        {
            _context.WorkGroupStaff.Add(staff);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStaffAsync(int id)
        {
            var staff = await _context.WorkGroupStaff.FirstOrDefaultAsync(s => s.Id == id);
            if (staff == null) return;
            _context.WorkGroupStaff.Remove(staff);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkGroupStaff>> GetAllStaffsAsync()
        {
            return await _context.WorkGroupStaff.AsNoTracking().ToListAsync();
        }

        public async Task<WorkGroupStaff?> GetOneStaffAsync(int id)
        {
            return await _context.WorkGroupStaff.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<WorkGroupStaff>> SearchStaffsAsync(string search = "", string workGroup = "")
        {
            var query = _context.WorkGroupStaff.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    (s.Name != null && EF.Functions.Like(s.Name, $"%{search}%")) ||
                    (s.Surname != null && EF.Functions.Like(s.Surname, $"%{search}%")));
            }

            if (!string.IsNullOrWhiteSpace(workGroup))
            {
                query = query.Where(s => s.WorkGroup != null && EF.Functions.Like(s.WorkGroup, $"%{workGroup}%"));
            }

            return await query.ToListAsync();
        }

        public async Task UpdateStaffAsync(WorkGroupStaff staff)
        {
            _context.WorkGroupStaff.Update(staff);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]> ExportToExcelAsync(string search, string workGroup)
        {
            var staffList = await SearchStaffsAsync(search, workGroup);
            
            return _excelService.GenerateExcel(staffList, "Çalışma Grubu Personel Listesi", (worksheet, data) =>
            {
                var headerRange = worksheet.Range(1, 1, 1, 6);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                worksheet.Cell(1, 1).Value = "Ad";
                worksheet.Cell(1, 2).Value = "Soyad";
                worksheet.Cell(1, 4).Value = "Çalışma Grubu";
                worksheet.Cell(1, 5).Value = "Ekip";
                worksheet.Cell(1, 6).Value = "Görev";
                worksheet.Cell(1, 3).Value = "Telefon";

                var row = 2;
                var staffListForExcel = data.ToList();
                foreach (var staff in staffListForExcel)
                {
                    worksheet.Cell(row, 1).Value = staff.Name ?? "";
                    worksheet.Cell(row, 2).Value = staff.Surname ?? "";
                    worksheet.Cell(row, 3).Value = staff.PhoneNumber ?? "";
                    worksheet.Cell(row, 4).Value = staff.WorkGroup ?? "";
                    worksheet.Cell(row, 5).Value = staff.Crew ?? "";
                    worksheet.Cell(row, 6).Value = staff.Duty ?? "";
                    row++;
                }

                if (staffListForExcel.Any())
                {
                    var dataRange = worksheet.Range(1, 1, staffListForExcel.Count + 1, 6);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Hair;
                }
            });
        }

        public async Task<ExcelImportResult> ImportFromExcelAsync(Stream stream)
        {
            var result = new ExcelImportResult();
            
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var reader = ExcelReaderFactory.CreateReader(stream);
            var ds = reader.AsDataSet();
            if (ds.Tables.Count == 0) return result;
            
            var table = ds.Tables[0];
            for (int i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                if (row.ItemArray.All(field => field == null || string.IsNullOrWhiteSpace(field.ToString()))) continue;

                try
                {
                    var staff = new WorkGroupStaff
                    {
                        Name = row[0]?.ToString()?.Trim() ?? string.Empty,
                        Surname = row[1]?.ToString()?.Trim() ?? string.Empty,
                        PhoneNumber = row[2]?.ToString()?.Trim(),
                        WorkGroup = row[3]?.ToString()?.Trim() ?? string.Empty,
                        Crew = row[4]?.ToString()?.Trim(),
                        Duty = row[5]?.ToString()?.Trim()
                    };

                    if (string.IsNullOrWhiteSpace(staff.Name) || string.IsNullOrWhiteSpace(staff.Surname))
                    {
                        result.AddError($"Satır {i + 1}: Ad ve Soyad boş olamaz.");
                        continue;
                    }

                    await CreateStaffAsync(staff);
                    result.AddSuccess();
                }
                catch (Exception ex)
                {
                    result.AddError($"Satır {i + 1}: {ex.Message}");
                }
            }

            return result;
        }
    }
}
