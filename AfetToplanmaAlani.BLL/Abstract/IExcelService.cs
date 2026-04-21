using System.Data;
using ClosedXML.Excel;

namespace AfetToplanmaAlani.BLL.Abstract
{
    public interface IExcelService
    {
        byte[] GenerateExcel<T>(IEnumerable<T> data, string sheetName, Action<IXLWorksheet, IEnumerable<T>> formatter);
        List<T> ParseExcel<T>(Stream stream, Func<DataRow, T> mapper);
    }
}
