using Notes.Model;
using Notes.Model.RequestResponse;

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
    Note newNote = new() { Id = Guid.NewGuid(), Title = request.Title };
    return Results.Created($"/notes/{newNote.Id}", newNote);
});

app.Run();

