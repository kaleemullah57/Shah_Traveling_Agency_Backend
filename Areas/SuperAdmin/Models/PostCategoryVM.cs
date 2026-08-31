namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class PostCategoryVM
    {
    }


    // Add Post Categories
    public class AddPostCategoryModel
    {
        public string CategoryName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }



    // Get Post Categories
    public class GetPostCategoryRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetPostCategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CreatedById { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
