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
        var createdNote = await AddNoteToServer(client, new NoteCreationRequests("Test Note", "Test Content"));
        var createdNoteLocation = await GetLocationOfResponse(createdNote);
        var createdNoteResponse = await GetNoteResponse(createdNote);

        var updatedTitle = "Updated test note";
        var updatedContent = "Updated note content";

        // Action: update the note
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
        var createdNoteLocation = await GetLocationOfResponse(createdNote);
        var createdNoteResponse = await GetNoteResponse(createdNote);
        var wrongGuid = Guid.NewGuid();
        Assert.NotEqual(createdNoteResponse.Id, wrongGuid);
        var updatedNote = new NoteUpdateRequest(Guid.NewGuid(), "updatedTitle", "updatedContent");

        //Action: update the note
        var response = await client.PutAsJsonAsync(createdNoteLocation, updatedNote);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var unprocessableEntity = await response.Content.ReadAsStringAsync();
        Assert.Contains("Id in body did not match id in link.", unprocessableEntity);

    }

    [Fact]
    public async Task PUT_UpdateANoteThatDoesntExsistServerEmpty_Returns404AndErrorMessage()
    {
        var client = Client;
        var noteGuid = Guid.NewGuid();
        var updatedNote = new NoteUpdateRequest(noteGuid, "updatedTitle", "updatedContent");

        var response = await client.PutAsJsonAsync($"/notes/{noteGuid}", updatedNote);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var notFound = await response.Content.ReadAsStringAsync();
        Assert.Contains("Note not found!", notFound);
    }

    [Fact]
    public async Task PUT_CreateSeveralNotesThenUpdateOneAndReadItBack_Returns200AndTheRightContentInBody()
    {
        var client = Client;

        var createdNotes = await FillServerWithNotes(client, notes);
        var locationOfThirdNote = await GetLocationOfResponse(createdNotes[2]);
        var thirdNoteResponse = await GetNoteResponse(createdNotes[2]);
        var updatedTitle = "Updated Title";
        var updatedContent = "Updated Content";
        var updatedNote = new NoteUpdateRequest(thirdNoteResponse.Id, updatedTitle, updatedContent);

        // Action
        var responseUpdateNote = await client.PutAsJsonAsync(locationOfThirdNote, updatedNote);
        responseUpdateNote.EnsureSuccessStatusCode();
        var responseReadUpdatedNote = await client.GetAsync(locationOfThirdNote);
        responseReadUpdatedNote.EnsureSuccessStatusCode();

        // Assert
        var noteResponse = await responseReadUpdatedNote.Content.ReadFromJsonAsync<NoteResponse>();

        Assert.NotNull(noteResponse);
        Assert.Equal(updatedNote.Id, noteResponse.Id);
        Assert.Equal(updatedTitle, noteResponse.Title);
        Assert.Equal(updatedContent, noteResponse.Content);
    }

    [Fact]
    public async Task PUT_CreateSeveralNotesThenUpdateOneThatDoesntExsist_Returns404AndErrorMessage()
    {
        var client = Client;

        // Fill server with notes and make a wrong GUID,
        var createdNotes = await FillServerWithNotes(client, notes);
        var wrongGuid = Guid.NewGuid();
        var updatedNote = new NoteUpdateRequest(wrongGuid, "Title of wrong update", "Content of wrong update");

        // make sure the notes on the server does not have the same GUID
        var createdNotesResponsesTask = createdNotes.Select(GetNoteResponse);
        var createdNotesResponses = await Task.WhenAll(createdNotesResponsesTask);
        var createdNotesGuids = createdNotesResponses.Select(item => item.Id).ToArray();
        Assert.DoesNotContain(wrongGuid, createdNotesGuids);

        // Action
        var response = await client.PutAsJsonAsync($"/notes/{wrongGuid}", updatedNote);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var notFound = await response.Content.ReadAsStringAsync();
        Assert.Contains("Note not found!", notFound);
    }
}