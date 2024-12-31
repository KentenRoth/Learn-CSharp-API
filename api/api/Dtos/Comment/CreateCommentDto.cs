using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment;

public class CreateCommentDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be 5 characters or more")]
    [MaxLength(50, ErrorMessage = "Title must be 50 characters or less")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MinLength(5, ErrorMessage = "Content must be 5 characters or more")]
    [MaxLength(280, ErrorMessage = "Content must be 280 characters or less")]
    public string Content { get; set; } = string.Empty;
}