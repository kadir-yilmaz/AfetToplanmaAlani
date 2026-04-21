using System.Data;
using AfetToplanmaAlani.BLL.Abstract;
using AfetToplanmaAlani.DAL;
using AfetToplanmaAlani.EL.Dtos;
using AfetToplanmaAlani.EL.Dtos.Staff;
using AfetToplanmaAlani.EL.Models;
using AutoMapper;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;

namespace AfetToplanmaAlani.BLL.Concrete
{
    public class StaffService : IStaffService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;

        public StaffService(AppDbContext context, IMapper mapper, IExcelService excelService)
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
        }

        public async Task CreateStaffAsync(StaffDtoForCreate staffDtoForCreate)
        {
            var staff = _mapper.Map<Staff>(staffDtoForCreate);
            _context.Staff.Add(staff);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStaffAsync(int id)
        {
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.Id == id);
            if (staff is null)
            {
                return;
            }

            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Staff>> GetAllStaffsAsync()
        {
            return await BuildStaffQuery().ToListAsync();
        }

        public async Task<IEnumerable<Staff>> SearchStaffsAsync(StaffSearchFilter filter)
        {
            var query = BuildStaffQuery();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(s =>
                    (s.Name != null && EF.Functions.Like(s.Name, $"%{filter.Search}%")) ||
                    (s.Surname != null && EF.Functions.Like(s.Surname, $"%{filter.Search}%")) ||
                    (s.PhoneNumber != null && EF.Functions.Like(s.PhoneNumber, $"%{filter.Search}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Place))
            {
                query = query.Where(s => s.Place != null && s.Place.Name != null && EF.Functions.Like(s.Place.Name, $"%{filter.Place}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                query = query.Where(s => s.Place != null && s.Place.City == filter.City);
            }

            if (!string.IsNullOrWhiteSpace(filter.District))
            {
                query = query.Where(s => s.Place != null && s.Place.District == filter.District);
            }

            if (!string.IsNullOrWhiteSpace(filter.Neighborhood))
            {
                query = query.Where(s => s.Place != null && s.Place.Neighborhood == filter.Neighborhood);
            }

            return await query
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Surname)
                .ToListAsync();
        }

        public async Task<Staff?> GetOneStaffAsync(int id)
        {
            return await BuildStaffQuery().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<StaffDtoForUpdate?> GetOneStaffForUpdateAsync(int id)
        {
            var staff = await _context.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            return staff is null ? null : _mapper.Map<StaffDtoForUpdate>(staff);
        }

        public async Task UpdateStaffAsync(StaffDtoForUpdate staffDtoForUpdate)
        {
            var entity = _mapper.Map<Staff>(staffDtoForUpdate);
            _context.Staff.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]> ExportToExcelAsync(StaffSearchFilter filter)
        {
            var staffList = await SearchStaffsAsync(filter);
            
            return _excelService.GenerateExcel(staffList, "Personel Listesi", (worksheet, data) =>
            {
                worksheet.Cell(1, 1).Value = "Ad";
                worksheet.Cell(1, 2).Value = "Soyad";
                worksheet.Cell(1, 3).Value = "Telefon";
                worksheet.Cell(1, 4).Value = "Toplanma Noktası";
                worksheet.Cell(1, 5).Value = "Şehir";
                worksheet.Cell(1, 6).Value = "İlçe";

                var row = 2;
                foreach (var staff in data)
                {
                    worksheet.Cell(row, 1).Value = staff.Name ?? "";
                    worksheet.Cell(row, 2).Value = staff.Surname ?? "";
                    worksheet.Cell(row, 3).Value = staff.PhoneNumber ?? "";
                    worksheet.Cell(row, 4).Value = staff.Place?.Name ?? "";
                    worksheet.Cell(row, 5).Value = staff.Place?.City ?? "";
                    worksheet.Cell(row, 6).Value = staff.Place?.District ?? "";
                    row++;
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
                    var dto = new StaffDtoForCreate
                    {
                        Name = row[0]?.ToString()?.Trim() ?? string.Empty,
                        Surname = row[1]?.ToString()?.Trim() ?? string.Empty,
                        PhoneNumber = row[2]?.ToString()?.Trim(),
                        PlaceId = null
                    };

                    if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Surname))
                    {
                        result.AddError($"Satır {i + 1}: Ad ve Soyad boş olamaz.");
                        continue;
                    }

                    await CreateStaffAsync(dto);
                    result.AddSuccess();
                }
                catch (Exception ex)
                {
                    result.AddError($"Satır {i + 1}: {ex.Message}");
                }
            }

            return result;
        }

        private IQueryable<Staff> BuildStaffQuery()
        {
            return _context.Staff
                .Include(s => s.Place)
                .AsNoTracking();
        }
    }
}
