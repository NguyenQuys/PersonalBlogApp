using PersonalBlogApp.Models;

public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Content { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public Guid? ParentId { get; set; }
    public Comment Parent { get; set; } // Self-reference

    public ICollection<Comment> Replies { get; set; } = new List<Comment>();

    public Guid BlogId { get; set; }
    public Blog Blog { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
