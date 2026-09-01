namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class DestinationVM
    {
    }



    // Add Destinations 
    public class AddDestinationModel
    {
        public string DestinationName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        public List<IFormFile>? PicturePath { get; set; }

        public int CountryId { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public bool IsActive { get; set; } = true;
    }



    // Get Destinations
    public class DestinationModel
    {
        public int DestinationId { get; set; }

        public string DestinationName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        public int CountryId { get; set; }

        public string CountryName { get; set; } = string.Empty;

        public int? ProvinceId { get; set; }

        public string? ProvinceName { get; set; }

        public int? CityId { get; set; }

        public string? CityName { get; set; }

        public List<string> PicturePath { get; set; } = new();

        public int CreatedById { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }
    }


}



