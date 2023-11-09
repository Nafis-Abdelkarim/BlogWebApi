namespace BlogWebApi.Models.ModelMapping
{
    public class PostDTO
    {
        public Guid PostId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? UserId { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
        public string? CategoryName { get; set; }
        public string? UserName { get; set; }

    }
}
