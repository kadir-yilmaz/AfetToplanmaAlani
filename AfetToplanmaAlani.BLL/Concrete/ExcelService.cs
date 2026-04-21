using System.Data;
using AfetToplanmaAlani.BLL.Abstract;
using ClosedXML.Excel;
using ExcelDataReader;

namespace AfetToplanmaAlani.BLL.Concrete
{
    public class ExcelService : IExcelService
    {
        public byte[] GenerateExcel<T>(IEnumerable<T> data, string sheetName, Action<IXLWorksheet, IEnumerable<T>> formatter)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);
            
            formatter(worksheet, data);
            
            worksheet.Columns().AdjustToContents();
            
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public List<T> ParseExcel<T>(Stream stream, Func<DataRow, T> mapper)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();

            if (result.Tables.Count == 0)
            {
                return new List<T>();
            }

            var table = result.Tables[0];
            var list = new List<T>();

            // Assume first row is header, start from index 1
            for (int i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                if (row.ItemArray.All(field => field == null || string.IsNullOrWhiteSpace(field.ToString())))
                {
                    continue;
                }

                try
                {
                    var item = mapper(row);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
                catch
                {
                    // Basic catch-all to skip corrupted rows, 
                    // higher level logic will handle specific errors if needed
                }
            }

            return list;
        }
    }
}
