using System.ComponentModel.DataAnnotations;

namespace Notes.Model.RequestResponse;

public record NoteCreationRequests([Required] string Title, string Content = "");