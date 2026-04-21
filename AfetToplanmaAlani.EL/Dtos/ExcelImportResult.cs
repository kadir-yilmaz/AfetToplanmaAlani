namespace AfetToplanmaAlani.EL.Dtos
{
    public class ExcelImportResult
    {
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        
        public void AddError(string message)
        {
            ErrorCount++;
            ErrorMessages.Add(message);
        }
        
        public void AddSuccess()
        {
            SuccessCount++;
        }
    }
}
