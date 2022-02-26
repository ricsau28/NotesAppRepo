using System;
using Microsoft.Data.Sqlite;

namespace NotesApp
{

    public class NoteArchiver
    {
        string dbFullNameAndPath;

        public NoteArchiver(string dbNameAndPath)
        {
            dbFullNameAndPath = dbNameAndPath;
        }

        private string CreateInsertSQL(Note note)
        {

            if (note == null || string.IsNullOrEmpty(dbFullNameAndPath))
                return string.Empty;

            //TODO: Set unique index on guid in table?
            /*string insertSQL = string.Format("INSERT INTO notes(guid, tomboy_version, title, note_version, note_text, note_content, " +
                                             "create_date, last_change_date, notebook_name) VALUES('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, {8})",
                                             note.GUID, note.Version, note.Title, note.NoteVersion, note.Text, note.Content,
                                             note.CreateDate, note.ChangeDate, note.Notebook);
            */

            string insertSQL = string.Format("INSERT INTO notes(guid, tomboy_version, title, note_version, " +
                                             "create_date, last_change_date, notebook_name, note_content, note_text) " +
                                             "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                                             note.GUID,
                                             note.Version,
                                             note.Title.Replace("'", "''"),
                                             note.NoteVersion,
                                             note.CreateDate,
                                             note.ChangeDate,
                                             note.Notebook,
                                             NotesAppUtilities.ReplaceSQLChars(note.Content),
                                             NotesAppUtilities.ReplaceSQLChars(note.Text));

            //Console.WriteLine("\nAddNoteToDB.InsertSQL: " + insertSQL);

            //string sqlNoteText = NotesAppUtilities.ReplaceSQLChars(note.Text);

            //string updateSQL = string.Format("UPDATE notes SET note_text='{0}' WHERE id = 1", sqlNoteText);

            //Console.WriteLine("\nAddNoteToDB.UpdateSQL: " + updateSQL);

            //AddNoteToDB(insertSQL);

            return insertSQL;

        }


        public void AddNotesToDB(List<Note> notes)
        {
            foreach (var note in notes)
            {
                AddNoteToDB(note);
            }
        }

        public bool AddNoteToDB(Note note)
        {
            if (note == null || string.IsNullOrEmpty(dbFullNameAndPath))
                return false;

            //TODO: Set unique index on guid in table?
            /*string insertSQL = string.Format("INSERT INTO notes(guid, tomboy_version, title, note_version, note_text, note_content, " +
                                             "create_date, last_change_date, notebook_name) VALUES('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, {8})",
                                             note.GUID, note.Version, note.Title, note.NoteVersion, note.Text, note.Content,
                                             note.CreateDate, note.ChangeDate, note.Notebook);
            */

            string insertSQL = CreateInsertSQL(note);
            AddNoteToDB(insertSQL);

            return false;

        }

        private void AddNoteToDB(string insertSQL)
        {
            Console.WriteLine("\n*** NoteArchiver.AddNoteToDB ***");

            string dataSource = string.Format("Data Source={0}", this.dbFullNameAndPath);
            int rowsInserted = -1;

            using (var connection = new SqliteConnection(dataSource))
            {
                connection.Open();

                var command = connection.CreateCommand();

                //command.CommandText = @"SELECT * FROM tomboy_notes";
                command.CommandText = insertSQL;

                try
                {
                    rowsInserted = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("NoteArchiver.AddNoteToDB: \n" + ex);
                }

            }// end using

            Console.WriteLine("NoteArchiver.AddNoteToDB: There was/were " + rowsInserted.ToString() + " row/rows inserted");

        }// end method

    }

}