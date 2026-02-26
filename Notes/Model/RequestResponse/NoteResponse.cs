using System.ComponentModel.DataAnnotations;

namespace Notes.Model.RequestResponse;

public record NoteResponse([Required] Guid Id, [Required] string Title, string Content = "");