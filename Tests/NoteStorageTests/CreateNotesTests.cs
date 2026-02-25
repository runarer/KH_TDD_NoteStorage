using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Model.RequestResponse;

namespace NoteStorageTests;

public class CreateNotesTests(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{



    [Fact]
    public async Task CreateNote_CreateANoteWithTitleAndContentOnAnEmptyServer_NoteShouldBeCreatedWithAGuiAndReturnedInBodyAndCode201()
    {
        var client = Client;
        string newNoteTitle = "Test Note Title";
        string newNoteContent = "Test note content";
        var noteToCreate = new NoteCreationRequests(newNoteTitle, newNoteContent);

        var response = await client.PostAsJsonAsync("/notes", noteToCreate);
        response.EnsureSuccessStatusCode();

        var createResponse = await response.Content.ReadFromJsonAsync<NoteCreationResponse>();
        Assert.NotNull(createResponse);
        Assert.NotEmpty(createResponse.Id);
        Assert.Equal(newNoteTitle, createResponse.Title);
        Assert.Equal(newNoteContent, createResponse.Content);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.Contains($"/notes/{createResponse.Id}", response.Headers.Location.ToString());
    }

    [Fact(Skip = "Waiting on implementation")]
    public void CreateNote_CreatesANoteWithoutTitle_ShouldNotBeCreatedAndReturn400AndErrorMessage()
    {
        Assert.Fail("Test not yet implemented");
    }

    [Fact(Skip = "Waiting on implementation")]
    public void CreateNote_CreateNoteWithTitleButNoContent_Return201AndNoteIsCreatedOnServer()
    {
        Assert.Fail("Test not yet implemented");
    }
}
