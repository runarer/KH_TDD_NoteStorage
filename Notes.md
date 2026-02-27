# Notes

## What is Notes

Notes is a API for handling Notes, a project for exploring TDD and
containerization.

The start is an implementation of a CRUD for notes.

## Process

1.  Write Healthcheck, if the server does not exists nothing will work.
2.  Identify core of the system.
    In this case a CRUD, especially Create and Read is the core.
    There are more wants and usefull thing, but they fall under "nice to haves"
    for now.
3.  Write tests for creation of a note (CREATE)
    1.  A note as expected -> should return 201 and a link to the resource.
    2.  A note without a title -> should return 400 and an appropriate
        message for the error
    3.  A note without a content -> 201 and return a link to the resource
        It's only a title that is required for creating a note. The body will
        be empty. The ide is to "make a note of a note" for later completion.
4.  Write tests for the consumption of notes (READ)
    1.  Create a note (this has been tested) and read it back
    2.  Try to read a note when there are non -> returns 404
    3.  Create several notes and read back all, check if they are the same.
        This test is make sure we do not overwrite notes and can keep them
        appart. This test can replace the first test, but it might be better to
        have both for the process?
    4.  Create some notes and try to read a non-exsisting note -> return 404,
        to make sure it doesn't just return "first note" or some random note.
5.  Write test for updating existing data (UPDATE)
    1.  Create a note, update it, read it back. -> Return 200 and the updated object
    2.  Update a note when non exsist -> Return 404 and error message
    3.  Create several notes, update one, read it back -> Return 200 and updated object
    4.  Create several notes, update a non exsisting one -> Return 404 and error message
    5.  Create a note then try to update it using wrong Id in body -> Return 422 and error message
6.  Write test for the deletion of notes (DELETE)
    1.  Delete a non-exsiting note on a empty server -> Return 404 and error message
    2.  Create several notes, delete a non-exsisting one -> Return 404 and error message
    3.  Create a note and delete it, try to read it. -> Return 204 on DELETE and 404 on GET.
    4.  Create several notes and delete on -> Return 204 on DELETE and 404 on GET.

### Discoveries

3.1 Need a NoteCreationRequest object
3.1 Need a NoteResponse object
3.3 This test is the same as 3.1 so use `[Theroy]` instead
5.1 Need a NoteUpdateRequest object
The whole test on empty server special case is probably redundant.

### Spørsmål

Å teste at et program gjør det det skal er en ting, å teste at programmet bare gjør det
det skal er en annen ting. I hvilken grad skal Unit Test teste nummer to.

Når jeg har en test for å lage noe på en server, jeg tester tilbakemeldinger som 201,
link i header og objektet i body. Hva med å teste at linken gitt i header returnerer
innholdet, eller er dette en test som hører inne under Read tester?
-> Hører under read!

Flere av testene er for tomme servere. Er dette noe man skal teste i denne sammenhengen?
Er 0,1,mange riktig tankegang.
