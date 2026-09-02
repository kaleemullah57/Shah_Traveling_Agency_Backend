using System.Text.Json;

namespace Shah_Traveling_Agency_API.Areas.PublicArea.Models
{
    public class PublicDestinationsVM
    {
    }



    // Get For Public
    public class GetPublicDestinationModel
    {
        public string? DestinationName { get; set; }

        public string? Description { get; set; }

        public string? PicturePathJson { get; set; }

        public List<string> PicturePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PicturePathJson))
                    return new List<string>();

                try
                {
                    return System.Text.Json.JsonSerializer
                        .Deserialize<List<string>>(PicturePathJson)
                        ?? new List<string>();
                }
                catch
                {
                    return new List<string>();
                }
            }
        }

        public string? CountryName { get; set; }

        public string? ProvinceName { get; set; }

        public string? CityName { get; set; }

        public string? BranchName { get; set; }

        public string? CreatedBy { get; set; }
    }
}
