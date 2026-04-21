using AfetToplanmaAlani.EL.Dtos;
using AfetToplanmaAlani.EL.Dtos.Staff;
using AfetToplanmaAlani.EL.Models;

namespace AfetToplanmaAlani.BLL.Abstract
{
    public interface IStaffService
    {
        Task<IEnumerable<Staff>> GetAllStaffsAsync();
        Task<IEnumerable<Staff>> SearchStaffsAsync(StaffSearchFilter filter);
        Task<Staff?> GetOneStaffAsync(int id);
        Task<StaffDtoForUpdate?> GetOneStaffForUpdateAsync(int id);
        Task CreateStaffAsync(StaffDtoForCreate staffDtoForCreate);
        Task UpdateStaffAsync(StaffDtoForUpdate staffDtoForUpdate);
        Task DeleteStaffAsync(int id);
        
        // Excel operations
        Task<byte[]> ExportToExcelAsync(StaffSearchFilter filter);
        Task<ExcelImportResult> ImportFromExcelAsync(Stream stream);
    }
}
