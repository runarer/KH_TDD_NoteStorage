using Notes.Model;
using Notes.Model.RequestResponse;

Dictionary<Guid, Note> noteStorage = [];

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health");


app.MapPost("/notes", (NoteCreationRequests request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest("Note needs a non empty title!");

    Note newNote = new() { Id = Guid.NewGuid(), Title = request.Title, Content = request.Content };

    noteStorage[newNote.Id] = newNote;

    return Results.Created($"/notes/{newNote.Id}", newNote);
});


app.MapGet("/notes/{id:Guid}", (Guid id) =>
{
    if (!noteStorage.TryGetValue(id, out Note? note))
        return Results.NotFound("Note not found!");

    return Results.Ok(note);
});

app.Run();

