/*
4.  Write tests for the consumption of notes (READ)
    1.  Create a note (this has been tested) and read it back
    2.  Try to read a note when there are non -> returns 404
    3.  Create several notes and read back all, check if they are the same.
        This test is make sure we do not overwrite notes and can keep them
        appart. This test can replace the first test, but it might be better to
        have both for the process?
    4.  Create some notes and try to read a non-exsisting note -> return 404,
        to make sure it doesn't just return "first note" or some random note.
*/

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Model.RequestResponse;

namespace NoteStorageTests;


public class ReadNotesTests(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{

    [Fact]
    public async Task GET_CreateANoteAndReadItBack_Returns200AndSameNoteInContentOfResponse()
    {
        var client = Client;
        var newNoteTitle = "Test Note";
        var newNoteContent = "Test Content";
        var newNote = new NoteCreationRequests(newNoteTitle, newNoteContent);
        var responseCreateNote = await client.PostAsJsonAsync("/notes", newNote);
        responseCreateNote.EnsureSuccessStatusCode();
        var newNoteLocation = responseCreateNote.Headers.Location;

        var responseReadNote = await client.GetAsync(newNoteLocation);

        responseReadNote.EnsureSuccessStatusCode();
        var response = await responseReadNote.Content.ReadFromJsonAsync<NoteResponse>();

        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(newNoteTitle, response.Title);
        Assert.Equal(newNoteContent, response.Content);
    }

    [Fact]
    public async Task GET_ReadANoteFromAnEmpptyServer_Returns404AndErrorMessage()
    {
        var client = Client;

        var response = await client.GetAsync("/notes/00000000-0010-0110-0230-000000000000");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var badRequest = await response.Content.ReadAsStringAsync();
        Assert.Contains("Note not found!", badRequest);

    }

    [Fact]
    public async Task GET_CreateSeveralNotesAndReadBackOne_Returns200AndRightContentInResponse()
    {
        var client = Client;
        var createdNotes = await FillServerWithNotes(client, notes);

        var responseReadNote = await client.GetAsync(createdNotes[2]);

        responseReadNote.EnsureSuccessStatusCode();
        var response = await responseReadNote.Content.ReadFromJsonAsync<NoteResponse>();

        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(notes[2].Title, response.Title);
        Assert.Equal(notes[2].Content, response.Content);
    }

    [Fact]
    public async Task GET_CreateSeveralNotesAndReadBackOneThatDoesntExists_Returns400AndErrorMessage()
    {
        var client = Client;
        _ = FillServerWithNotes(client, notes);

        var response = await client.GetAsync("/notes/00000000-0010-0110-0230-000000000000");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var badRequest = await response.Content.ReadAsStringAsync();
        Assert.Contains("Note not found!", badRequest);

    }

    private async Task<List<Uri>> FillServerWithNotes(HttpClient client, NoteCreationRequests[] notes)
    {
        List<Uri> noteLocations = [];
        for (int i = 0; i < notes.Length; i++)
        {
            var response = await client.PostAsJsonAsync("/notes", notes[i]);
            response.EnsureSuccessStatusCode();

            var noteLocation = response.Headers.Location;
            Assert.NotNull(noteLocation);
            noteLocations.Add(noteLocation);
        }
        return noteLocations;
    }

    private readonly NoteCreationRequests[] notes =
    [
        new NoteCreationRequests("Test Note 0 Title","Test Note 0 Content"),
        new NoteCreationRequests("Test Note 1 Title","Test Note 1 Content"),
        new NoteCreationRequests("Test Note 2 Title","Test Note 2 Content"),
        new NoteCreationRequests("Test Note 3 Title","Test Note 3 Content"),
        new NoteCreationRequests("Test Note 4 Title","Test Note 4 Content"),
    ];

}