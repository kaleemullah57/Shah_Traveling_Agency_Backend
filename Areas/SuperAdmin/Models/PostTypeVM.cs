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
}
