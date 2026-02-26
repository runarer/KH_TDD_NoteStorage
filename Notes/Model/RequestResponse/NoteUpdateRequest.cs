using System.ComponentModel.DataAnnotations;

namespace Notes.Model.RequestResponse;

public record NoteUpdateRequest([Required] Guid Id, [Required] string Title, [Required] string Content);