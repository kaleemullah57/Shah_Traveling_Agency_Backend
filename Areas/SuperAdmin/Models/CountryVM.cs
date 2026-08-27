namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class CountryVM
    {
        public string? search { get; set; }
        public int pageNumber { get; set; }
        public int pageSize{ get; set; }
    }

    public class Country
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public bool IsActive { get; set; }
        public int CreatedById { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedOn { get; set; }
    }




    // Add Countries
    public class AddCountryRequest
    {
        public string CountryName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
