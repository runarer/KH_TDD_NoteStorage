using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Model.RequestResponse;

namespace NoteStorageTests;

public class CreateNotesTests(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{

    [Theory]
    [InlineData("Test Note Title", "Test Note Content")]
    [InlineData("Test Note Title", "")]
    public async Task CreateNote_CreateANoteWithTitleAndContentOnAnEmptyServer_NoteShouldBeCreatedWithAGuiAndReturnedInBodyAndCode201(string newNoteTitle, string newNoteContent)
    {
        var client = Client;
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

    [Fact]
    public async Task CreateNote_CreatesANoteWithoutTitle_ShouldNotBeCreatedAndReturn400AndErrorMessage()
    {
        var client = Client;
        var noteNotToCreate = new NoteCreationRequests("", "Test Content");

        var response = await client.PostAsJsonAsync("/notes", noteNotToCreate);


        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var badRequest = await response.Content.ReadAsStringAsync();
        Assert.Contains("Note needs a non empty title!", badRequest);
    }
}
