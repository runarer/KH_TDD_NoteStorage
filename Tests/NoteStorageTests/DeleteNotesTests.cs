/*
6.  Write test for the deletion of notes (DELETE)
    1.  Delete a non-exsiting note on a empty server -> Return 404 and error message
    2.  Create several notes, delete a non-exsisting one -> Return 404 and error message
        Make sure all notes exists.
    3.  Create several notes and delete one -> Return 204 on DELETE and 404 on GET.
        Make sure the other notes still exsists
*/

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace NoteStorageTests;

public class DeleteNotesTests(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact]
    public async Task DELETE_DeleteANonExsistingNoteFromAnEmptyServer_Returns404AndAnErrorMessage()
    {
        var client = Client;
        var guid = Guid.NewGuid();

        var response = await client.DeleteAsync($"/notes/{guid}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var badRequest = await response.Content.ReadAsStringAsync();
        Assert.Contains("Note not found!", badRequest);

    }

    [Fact]
    public async Task DELETE_CreateSeveralNotesThenDeleteANonExsistingOne_Returns404AndAnErrorMessage()
    {
        var client = Client;

        // Fill server with notes and make a wrong GUID,
        var createdNotes = await FillServerWithNotes(client, notes);
        var wrongGuid = Guid.NewGuid();

        // make sure the notes on the server does not have the same GUID
        var createdNotesResponsesTask = createdNotes.Select(GetNoteResponse);
        var createdNotesResponses = await Task.WhenAll(createdNotesResponsesTask);
        var createdNotesGuids = createdNotesResponses.Select(item => item.Id).ToArray();
        Assert.DoesNotContain(wrongGuid, createdNotesGuids);

        // Action
        var response = await client.DeleteAsync($"/notes/{wrongGuid}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var notFound = await response.Content.ReadAsStringAsync();
        Assert.Contains("Note not found!", notFound);
    }

    [Fact]
    public async Task DELETE_CreateSeveralNotesDeleteOne_Returns200()
    {
        var client = Client;

        var createdNotes = await FillServerWithNotes(client, notes);
        var locationOfThirdNote = await GetLocationOfResponse(createdNotes[2]);

        // Action
        var response = await client.DeleteAsync(locationOfThirdNote);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseReadDeletedNote = await client.GetAsync(locationOfThirdNote);

        Assert.Equal(HttpStatusCode.NotFound, responseReadDeletedNote.StatusCode);
        var notFound = await responseReadDeletedNote.Content.ReadAsStringAsync();
        Assert.Contains("Note not found!", notFound);
    }
}