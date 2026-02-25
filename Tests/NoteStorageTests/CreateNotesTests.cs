namespace NoteStorageTests;

public class CreateNotesTests
{
    [Fact]
    public void CreateNote_CreateANoteWithTitleAndContentOnAnEmptyServer_NoteShouldBeCreatedAndReturnedInBodyAndCode201()
    {

    }
    [Fact]
    public void CreateNote_CreatesANoteWithoutTitle_ShouldNotBeCreatedAndReturn400AndErrorMessage()
    {

    }

    [Fact]
    public void CreateNote_CreateNoteWithTitleButNoContent_Return201AndNoteIsCreatedOnServer()
    {

    }
}
