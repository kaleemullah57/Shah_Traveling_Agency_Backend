namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class PostTypeVM
    {
    }




    // Add Post Types
    public class AddPostTypeModel
    {
        public string PostTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }



    // Get Post Types
    public class GetTravelTypeRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
    public class TravelTypeModel
    {
        public int PostTypeId { get; set; }
        public string PostTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CreatedByid { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
