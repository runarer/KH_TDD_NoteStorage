namespace NoteStorageTests;

public class CreateNotesTests
{
    [Fact(Skip = "Waiting on implementation")]
    public void CreateNote_CreateANoteWithTitleAndContentOnAnEmptyServer_NoteShouldBeCreatedAndReturnedInBodyAndCode201()
    {
        Assert.Fail("Test not yet implemented");
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
