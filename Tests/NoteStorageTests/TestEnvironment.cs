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

    protected static async Task<List<Uri>> FillServerWithNotes(HttpClient client, NoteCreationRequests[] notes)
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

    protected readonly NoteCreationRequests[] notes =
    [
        new NoteCreationRequests("Test Note 0 Title","Test Note 0 Content"),
        new NoteCreationRequests("Test Note 1 Title","Test Note 1 Content"),
        new NoteCreationRequests("Test Note 2 Title","Test Note 2 Content"),
        new NoteCreationRequests("Test Note 3 Title","Test Note 3 Content"),
        new NoteCreationRequests("Test Note 4 Title","Test Note 4 Content"),
    ];

}