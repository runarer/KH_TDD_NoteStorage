/*
5.  Write test for updating existing data (UPDATE)
    1.  Create a note, update it, read it back. -> Return 200 and the updated object
    2.  Update a note when non exsist -> Return 404 and error message
    3.  Create several notes, update one, read it back -> Return 200 and updated object
    4.  Create several notes, update a non exsisting one
*/

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Model.RequestResponse;

namespace NoteStorageTests;

public class UpdateNotesTests(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact]
    public async Task PUT_CreateANoteThenUpdateItAndReadItBack_Returns200AndTheRightContentInBody()
    {
        var client = Client;

        //Create a note and get its location and content
        var createdNote = await client.PostAsJsonAsync("/notes", new NoteCreationRequests("Test Note", "Test Content"));
        createdNote.EnsureSuccessStatusCode();
        Assert.NotNull(createdNote.Headers.Location);
        var createdNoteLocation = createdNote.Headers.Location;
        var createdNoteResponse = await createdNote.Content.ReadFromJsonAsync<NoteResponse>();
        Assert.NotNull(createdNoteResponse);

        var updatedTitle = "Updated test note";
        var updatedContent = "Updated note content";

        //Action: update the note
        var updatedNote = new NoteUpdateRequest(createdNoteResponse.Id, updatedTitle, updatedContent);

        // Assert
        var responseUpdateNote = await client.PutAsJsonAsync(createdNoteLocation, updatedNote);
        responseUpdateNote.EnsureSuccessStatusCode();

        var responseReadUpdatedNote = await client.GetAsync(createdNoteLocation);
        responseReadUpdatedNote.EnsureSuccessStatusCode();

        var noteResponse = await responseReadUpdatedNote.Content.ReadFromJsonAsync<NoteResponse>();

        Assert.NotNull(noteResponse);
        Assert.NotEqual(Guid.Empty, noteResponse.Id);
        Assert.Equal(updatedTitle, noteResponse.Title);
        Assert.Equal(updatedContent, noteResponse.Content);
    }

    [Fact]
    public async Task PUT_CreateANoteThenUpdateItWithWrongGuid_Returns422AndErrorMessage()
    {
        var client = Client;
        var createdNote = await AddNoteToServer(client, new NoteCreationRequests("Test Note", "Test Content"));
        var updatedNote = new NoteUpdateRequest(Guid.NewGuid(), "updatedTitle", "updatedContent");

        //Action: update the note
        var response = await client.PutAsJsonAsync(createdNote, updatedNote);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var badRequest = await response.Content.ReadAsStringAsync();
        Assert.Contains("Id in body did not match id in link.", badRequest);

    }

    [Fact(Skip = "Not implemented yet")]
    public async Task PUT_UpdateANoteThatDoesntExsistServerEmpty_Returns404AndErrorMessage()
    {

    }

    [Fact(Skip = "Not implemented yet")]
    public async Task PUT_CreateSeveralNotesThenUpdateOneAndReadItBack_Returns200AndTheRightContentInBody()
    {

    }


    [Fact(Skip = "Not implemented yet")]
    public async Task PUT_CreateSeveralNotesThenUpdateOneThatDoesntExsist_Returns404AndErrorMessage()
    {

    }
}