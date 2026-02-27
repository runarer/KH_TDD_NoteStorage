

using Microsoft.EntityFrameworkCore;
using Notes.Model;

namespace NoteStorage.Context;

public class NoteStorageDbContext(DbContextOptions<NoteStorageDbContext> options) : DbContext(options)
{
    public DbSet<Note> Notes { get; set; }
}