using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Model.RequestResponse;

namespace NoteStorageTests;

public class CreateNotesTests(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{



    [Fact]
    public async Task CreateNote_CreateANoteWithTitleAndContentOnAnEmptyServer_NoteShouldBeCreatedAndReturnedInBodyAndCode201()
    {
        var client = Client;
        var noteToCreate = new NoteCreationRequests("Test Note title", "Test note content");

        var response = await client.PostAsJsonAsync("/notes", noteToCreate);
        response.EnsureSuccessStatusCode();

        var createResponse = await response.Content.ReadFromJsonAsync<NoteCreationResponse>();
        Assert.NotNull(createResponse);
        Assert.NotEmpty(createResponse.Id);
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
