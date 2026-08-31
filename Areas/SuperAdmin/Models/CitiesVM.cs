namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class CitiesVM
    {
    }


    // Add Cities
    public class AddCityModel
    {
        public string CityName { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
