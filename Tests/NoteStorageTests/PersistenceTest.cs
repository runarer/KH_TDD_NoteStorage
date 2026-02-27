using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Model.RequestResponse;

namespace NoteStorageTests;

public class PersistenceTests(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{

    [Fact]
    public async Task CreateNote_CreateANoteRestartsTheServerReadNote_NoteShouldBeReadBackAfterRestart()
    {
        var client = Client;
        var noteTitle = "Test Note Title";
        var noteContent = "Test Note Content";
        var noteToCreate = new NoteCreationRequests(noteTitle, noteContent);
        var response = await AddNoteToServer(client, noteToCreate);
        response.EnsureSuccessStatusCode();
        var noteLocation = await GetLocationOfResponse(response);

        await RestartAsync();
        client = Client;

        var newResponse = await client.GetAsync(noteLocation);
        newResponse.EnsureSuccessStatusCode();
        var newNoteResponse = await GetNoteResponse(newResponse);

        Assert.Equal(noteTitle, newNoteResponse.Title);
        Assert.Equal(noteContent, newNoteResponse.Content);
    }

}
