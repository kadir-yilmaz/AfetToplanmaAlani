namespace AfetToplanmaAlani.BLL.Abstract
{
    public interface ILocationService
    {
        Task<IReadOnlyList<string>> GetCitiesAsync();
        Task<IReadOnlyList<string>> GetDistrictsAsync(string city = "");
        Task<IReadOnlyList<string>> GetNeighborhoodsAsync(string city = "", string district = "");
    }
}
