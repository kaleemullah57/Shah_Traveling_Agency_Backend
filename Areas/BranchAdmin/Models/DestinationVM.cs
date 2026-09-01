namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class DestinationVM
    {
    }



    // Add Destinations 
    public class AddDestinationModel
    {
        public string DestinationName { get; set; } = string.Empty;

        public List<IFormFile>? PicturePath { get; set; }

        public int CountryId { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public bool IsActive { get; set; } = true;
    }




}

