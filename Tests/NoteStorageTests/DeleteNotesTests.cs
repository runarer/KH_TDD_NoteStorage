/*
6.  Write test for the deletion of notes (DELETE)
    1.  Delete a non-exsiting note on a empty server -> Return 404 and error message
    2.  Create several notes, delete a non-exsisting one -> Return 404 and error message
        Make sure all notes exists.
    3.  Create several notes and delete one -> Return 204 on DELETE and 404 on GET.
        Make sure the other notes still exsists
*/

using Microsoft.AspNetCore.Mvc.Testing;

namespace NoteStorageTests;

public class DeleteNotesTests(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact(Skip = "Not implenented yet")]
    public async Task DELETE_DeleteANonExsistingNoteFromAnEmptyServer_Returns404AndAnErrorMessage()
    {

    }

    [Fact(Skip = "Not implenented yet")]
    public async Task DELETE_CreateSeveralNotesThenDeleteANonExsistingOne_Returns404AndAnErrorMessageAndAllNotesStillExsists()
    {

    }

    [Fact(Skip = "Not implenented yet")]
    public async Task DELETE_CreateSeveralNotesDeleteOne_Returns200AndOtherNotesStillExsists()
    {

    }


}