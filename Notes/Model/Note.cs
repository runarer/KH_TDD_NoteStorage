using System.ComponentModel.DataAnnotations;
using Notes.Model.RequestResponse;

namespace Notes.Model;

public class Note
{
    [Key]
    public required Guid Id { get; init; }
    [Required]
    public required string Title { get; set; }
    public string Content { get; set; } = string.Empty;
}