using AfetToplanmaAlani.EL.Dtos;
using AfetToplanmaAlani.EL.Models;

namespace AfetToplanmaAlani.BLL.Abstract
{
    public interface IWorkGroupStaffService
    {
        Task<IEnumerable<WorkGroupStaff>> GetAllStaffsAsync();
        Task<IEnumerable<WorkGroupStaff>> SearchStaffsAsync(string search = "", string workGroup = "");
        Task<WorkGroupStaff?> GetOneStaffAsync(int id);
        Task CreateStaffAsync(WorkGroupStaff staff);
        Task UpdateStaffAsync(WorkGroupStaff staff);
        Task DeleteStaffAsync(int id);
        
        // Excel operations
        Task<byte[]> ExportToExcelAsync(string search, string workGroup);
        Task<ExcelImportResult> ImportFromExcelAsync(Stream stream);
    }
}
