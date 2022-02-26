using System;

namespace NotesApp
{
    public class NotesController
    {

        public static string NotesFolder { get; set; }
        public static string SqlLiteDBName { get; set; }

        public static void InitApp()
        {
            NotesManager notesManager = new NotesManager(NotesFolder);
            NoteArchiver noteArchiver = new NoteArchiver(SqlLiteDBName);

            List<Note> noteList = notesManager.LoadNotes();

            noteArchiver.AddNotesToDB(noteList);

        }//end InitApp

    }


}