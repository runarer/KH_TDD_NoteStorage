using System.ComponentModel.DataAnnotations;

namespace Notes.Model;

class Note
{
    [Key]
    public required string Id { get; init; }
    [Required]
    public required string Title { get; set; }
    public string Content { get; set; } = string.Empty;
}