

using Microsoft.EntityFrameworkCore;
using Notes.Model;
using Notes.Model.RequestResponse;
using NoteStorage.Context;

namespace NoteStorage.Services;

public class NoteStorageService
{
    private readonly NoteStorageDbContext _context;
    public NoteStorageService(NoteStorageDbContext context)
    {
        _context = context;
    }

    public NoteResponse AddNote(NoteCreationRequests request)
    {
        var note = new Note { Id = Guid.NewGuid(), Title = request.Title, Content = request.Content };

        _context.Notes.Add(note);
        _context.SaveChanges();

        return new NoteResponse(note.Id, note.Title, note.Content);
    }

    public NoteResponse? ReadNote(Guid id)
    {
        var note = _context.Notes.FirstOrDefault(item => item.Id == id);
        return (note is null) ? null : new NoteResponse(note.Id, note.Title, note.Content);
    }
    public NoteResponse? UpdateNote(NoteUpdateRequest request)
    {
        var note = _context.Notes.FirstOrDefault(item => item.Id == request.Id);
        if (note is null) return null;

        note.Title = request.Title;
        note.Content = request.Content;
        _context.SaveChanges();
        return new NoteResponse(note.Id, note.Title, note.Content);
    }

    public bool DeleteNote(Guid id)
    {
        int rowsDeleted = _context.Notes.Where(e => e.Id == id).ExecuteDelete();
        return rowsDeleted > 0;
    }

}