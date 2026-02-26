/*
5.  Write test for updating existing data (UPDATE)
    1.  Create a note, update it, read it back. -> Return 200 and the updated object
    2.  Update a note when non exsist -> Return 404 and error message
    3.  Create several notes, update one, read it back -> Return 200 and updated object
    4.  Create several notes, update a non exsisting one
*/

using Microsoft.AspNetCore.Mvc.Testing;

namespace NoteStorageTests;

public class UpdateNotesTests(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact(Skip = "Not implemented yet")]
    public async Task PUT_CreateANoteThenUpdateItAndReadItBack_Returns200AndTheRightContentInBody()
    {

    }

    [Fact(Skip = "Not implemented yet")]
    public async Task PUT_UpdateANoteThatDoesntExsistServerEmpty_Returns404AndErrorMessage()
    {

    }

    [Fact(Skip = "Not implemented yet")]
    public async Task PUT_CreateSeveralNotesThenUpdateOneAndReadItBack_Returns200AndTheRightContentInBody()
    {

    }


    [Fact(Skip = "Not implemented yet")]
    public async Task PUT_CreateSeveralNotesThenUpdateOneThatDoesntExsist_Returns404AndErrorMessage()
    {

    }
}