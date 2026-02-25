using System.ComponentModel.DataAnnotations;

namespace Notes.Model.RequestResponse;

public record NoteCreationResponse([Required] string Id, [Required] string Title, string Content = "");