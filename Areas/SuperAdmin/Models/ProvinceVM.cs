namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class ProvinceVM
    {
    }

    public class AddProvinceModel
    {
        public string ProvinceName { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
