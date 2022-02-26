#define WINDOWS
#define LINUX
//#define USE_LOCAL

using System;
//using NotesApp;

namespace NotesApp
{

    class Program
    {
        static void Main(string[] args)
        {
            // See https://aka.ms/new-console-template for more information
            //Console.WriteLine("Program class says: Hello, World!");

            ///mnt/shared_linux/TomboyNotes
            //string tomboyNotesFolder = @"/home/ricsau/Documents/TomboyNotes";

#if WINDOWS
            NotesController.NotesFolder = @"H:\Programming\Copy of Linux Local Folder\TomboyConsole\Tomboy_Notes";
#endif

#if LINUX
            NotesController.NotesFolder = @"/mnt/shared_linux/TomboyNotes";
#endif

#if USE_LOCAL
            NotesController.NotesFolder = @"./Tomboy_Notes";
#endif

            NotesController.SqlLiteDBName = @"./TomboyNotes.db";

            Console.WriteLine("\nThe folder for notes is: " + NotesController.NotesFolder);
            Console.WriteLine("The database is: " + NotesController.SqlLiteDBName);

            /*
            NotesManager notesManager = new NotesManager(NotesController.NotesFolder);
            Note randomNote = notesManager.ReadNoteFromXML();

            Console.WriteLine("Contents of random note below...");
            Console.WriteLine(randomNote);

            var archiver = new NoteArchiver(NotesController.SqlLiteDBName);

            //NoteArchiver.Test(NotesApp.SqlLiteDBName);
            archiver.AddNoteToDB(randomNote);
            */

            NotesController.InitApp();
        }
    }

}