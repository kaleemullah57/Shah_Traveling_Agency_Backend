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



    // Get Cities
    public class GetCitiesRequestModel
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class CityResponseModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;

        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;

        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
        public int CreatedById { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
