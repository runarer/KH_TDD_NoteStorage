using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Notes.Model.RequestResponse;

namespace NoteStorageTests;

public class TestEnvironment : IClassFixture<WebApplicationFactory<Program>>
{
    protected WebApplicationFactory<Program> _factory;

    public TestEnvironment(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    public HttpClient Client => _factory.CreateClient();

    public async Task RestartAsync()
    {
        await _factory.DisposeAsync();
        _factory = new WebApplicationFactory<Program>();
    }

    protected static async Task<List<HttpResponseMessage>> FillServerWithNotes(HttpClient client, NoteCreationRequests[] notes)
    {
        var tasks = notes.Select(async note =>
        {
            var addedNote = await AddNoteToServer(client, note);
            return addedNote;
        });

        var results = await Task.WhenAll(tasks);

        return [.. results];
    }

    protected static async Task<HttpResponseMessage> AddNoteToServer(HttpClient client, NoteCreationRequests note)
    {
        var responseCreateNote = await client.PostAsJsonAsync("/notes", note);
        responseCreateNote.EnsureSuccessStatusCode();

        return responseCreateNote;
    }

    protected static async Task<Uri> GetLocationOfResponse(HttpResponseMessage response)
    {
        Assert.NotNull(response.Headers.Location);
        return response.Headers.Location;
    }

    protected static async Task<NoteResponse> GetNoteResponse(HttpResponseMessage response)
    {
        var createdNoteResponse = await response.Content.ReadFromJsonAsync<NoteResponse>();
        Assert.NotNull(createdNoteResponse);
        return createdNoteResponse;
    }

    protected readonly NoteCreationRequests[] notes =
    [
        new NoteCreationRequests("Test Note 0 Title","Test Note 0 Content"),
        new NoteCreationRequests("Test Note 1 Title","Test Note 1 Content"),
        new NoteCreationRequests("Test Note 2 Title","Test Note 2 Content"),
        new NoteCreationRequests("Test Note 3 Title","Test Note 3 Content"),
        new NoteCreationRequests("Test Note 4 Title","Test Note 4 Content"),
    ];

}