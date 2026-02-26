/*
4.  Write tests for the consumption of notes (READ)
    1.  Create a note (this has been tested) and read it back
    2.  Try to read a note when there are non -> returns 404
    3.  Create several notes and read back all, check if they are the same.
        This test is make sure we do not overwrite notes and can keep them
        appart. This test can replace the first test, but it might be better to
        have both for the process?
    4.  Create some notes and try to read a non-exsisting note -> return 404,
        to make sure it doesn't just return "first note" or some random note.
*/

using Microsoft.AspNetCore.Mvc.Testing;

namespace NoteStorageTests;


public class ReadNotesTests(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{

    [Fact(Skip = "Not implemented yet")]
    public async Task GET_CreateANoteAndReadItBack_Returns200AndSameNoteInContentOfResponse()
    {

    }

    [Fact(Skip = "Not implemented yet")]
    public async Task GET_ReadANoteFromAnEmpptyServer_Returns400AndErrorMessage()
    {

    }

    [Fact(Skip = "Not implemented yet")]
    public async Task GET_CreateSeveralNotesAndReadBackOne_Returns200AndRightContentInResponse()
    {

    }

    [Fact(Skip = "Not implemented yet")]
    public async Task GET_CreateSeveralNotesAndReadBackOneThatDoesntExists_Returns400AndErrorMessage()
    {

    }

}