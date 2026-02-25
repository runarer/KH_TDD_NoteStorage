using System.ComponentModel.DataAnnotations;

namespace Notes.Model.Request;

public record NoteCreationRequests([Required] string Title, string Content = "");