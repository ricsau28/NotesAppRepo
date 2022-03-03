using System;
using Microsoft.Data.Sqlite;

namespace NotesApp
{

    public class NoteArchiver
    {
        //string dbFullNameAndPath;
        string dataSource;

        public NoteArchiver(string dbNameAndPath)
        {
            if (string.IsNullOrEmpty(dbNameAndPath))
            {
                throw new ArgumentException("NoteArchiver: parameter dbNameAndPath is null or empty");
            }

            //dbFullNameAndPath = dbNameAndPath;

            dataSource = string.Format("Data Source={0}", dbNameAndPath);
        }

        private long AddNotebookToDB(string notebookName)
        {

            int rowsInserted = -1;
            long newRowID = -1;

            using (var cn = new SqliteConnection(dataSource))
            {
                cn.Open();

                string notebook = NotesAppUtilities.ReplaceSQLChars(notebookName);

                var cmd = cn.CreateCommand();

                cmd.CommandText = "INSERT INTO notebooks (name) VALUES (@notebookName)";
                cmd.Parameters.AddWithValue("@notebookName", notebook);

                try
                {
                    rowsInserted = cmd.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {

                    if (ex.ErrorCode == -2147467259)
                        goto Exit;
                    else
                    {
                        LogError("NoteArchiver.AddNotebookToDB: " + ex);
                        Console.WriteLine("NoteArchiver.AddNotebookToDB: {0}", ex);
                    }
                }

                cmd = cn.CreateCommand();
                cmd.CommandText = "SELECT last_insert_rowid()";

                try
                {
                    newRowID = (long)cmd.ExecuteScalar();
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine("NoteArchiver.AddNotebookToDB: {0}", ex);
                    throw ex;
                }
            }

        Exit:
            return newRowID;

        }// end method

        public long AddNoteToDB(Note noteToAdd)
        {

            int rowsInserted = -1;
            long newRowID = -1;

            //string dataSource = string.Format("Data Source={0}", this.dbFullNameAndPath);

            using (var cn = new SqliteConnection(dataSource))
            {
                cn.Open();

                var cmd = cn.CreateCommand();

                cmd.CommandText = CreateInsertSQL(noteToAdd);
                cmd.Parameters.AddWithValue("@guid", noteToAdd.GUID);
                cmd.Parameters.AddWithValue("@version", noteToAdd.Version);
                cmd.Parameters.AddWithValue("@title", NotesAppUtilities.ReplaceSQLChars(noteToAdd.Title));
                cmd.Parameters.AddWithValue("@noteVersion", noteToAdd.NoteVersion);
                cmd.Parameters.AddWithValue("@dateCreated", noteToAdd.CreateDate);
                cmd.Parameters.AddWithValue("@dateChanged", noteToAdd.ChangeDate);
                cmd.Parameters.AddWithValue("@notebook", NotesAppUtilities.ReplaceSQLChars(noteToAdd.Notebook));
                cmd.Parameters.AddWithValue("@content", NotesAppUtilities.ReplaceSQLChars(noteToAdd.Content));
                cmd.Parameters.AddWithValue("@text", NotesAppUtilities.ReplaceSQLChars(noteToAdd.Text));

                try
                {
                    rowsInserted = cmd.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    string errorMsg = string.Format("Couldn''t add note to database\nGUID: {0}\n" +
                        "Title: {1}\nText: {2}\nContent: {3}", noteToAdd.GUID, noteToAdd.Title, noteToAdd.Content, noteToAdd.Text);

                    LogError(errorMsg);

                    //Console.WriteLine("NoteArchiver.AddNoteToDB: Couldn't add note {0} to database...", noteToAdd.GUID);
                    Console.WriteLine(errorMsg);
                    Console.WriteLine("NoteArchiver.AddNoteToDB: insertSQL: {0}\n", cmd.CommandText);
                    Console.WriteLine(ex);
                    System.Diagnostics.Debugger.Break();
                }

                //Console.WriteLine("NoteArchiver.AddNoteToDB: There was/were " + rowsInserted.ToString() + " row/rows inserted");

                //Let's see if we can get the last _id from sqlite
                cmd = cn.CreateCommand();
                cmd.CommandText = "SELECT last_insert_rowid()";

                try
                {
                    newRowID = (long)cmd.ExecuteScalar();
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine("AddNoteToDB: Exception occurred: {0}", ex);
                }

                //Console.WriteLine("AddNoteToDB: Row id from sqlite: {0}", newRowID.ToString());
                return newRowID;

            }// end using

        } //end method

        private string CreateInsertSQL(Note note)
        {
            //TODO: Set unique index on guid in table?
            /*string insertSQL = string.Format("INSERT INTO notes(guid, tomboy_version, title, note_version, note_text, note_content, " +
                                             "create_date, last_change_date, notebook_name) VALUES('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, {8})",
                                             note.GUID, note.Version, note.Title, note.NoteVersion, note.Text, note.Content,
                                             note.CreateDate, note.ChangeDate, note.Notebook);
            */

            //string notebook = string.Empty;

            //if (note.Tags.Count < 1)
            //note.Notebook = "Unfiled";

            //if (note.Title == @"To Do's")
            //    System.Diagnostics.Debugger.Break();

            /*

            string insertSQL = string.Format("INSERT INTO notes(guid, tomboy_version, title, note_version, " +
                                             "create_date, last_change_date, notebook_name, note_content, note_text) " +
                                             "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                                             note.GUID,
                                             note.Version,
                                             NotesAppUtilities.ReplaceSQLChars(note.Title),
                                             note.NoteVersion,
                                             note.CreateDate,
                                             note.ChangeDate,
                                             note.Notebook,
                                             //note.Tags.Count <= 0 ? "Unfiled" : NotesAppUtilities.ReplaceSQLChars(note.Notebook),
                                             //NotesAppUtilities.ReplaceSQLChars(note.Notebook), //note.Notebook,
                                             NotesAppUtilities.ReplaceSQLChars(note.Content),
                                             NotesAppUtilities.ReplaceSQLChars(note.Text));

            */

            string insertSQL = string.Format("INSERT INTO notes(guid, tomboy_version, title, note_version, " +
                                             "create_date, last_change_date, notebook_name, note_content, note_text) " +
                                             "VALUES(@guid, @version, @title, @noteVersion, @dateCreated, " +
                                             "@dateChanged, @notebook, @content, @text)");

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
                AddNotebookToDB(note.Notebook);
            }
        }

        public void LogError(string errorMsg)
        {
            if (!string.IsNullOrEmpty(errorMsg))
            {
                using (var cn = new SqliteConnection(dataSource))
                {
                    cn.Open();

                    var cmd = cn.CreateCommand();

                    cmd.CommandText = "INSERT INTO error_log (message, date_occurred) VALUES (@message, @dateTimeOccurred)";
                    cmd.Parameters.AddWithValue("@message", errorMsg);
                    cmd.Parameters.AddWithValue("@dateTimeOccurred", NotesAppUtilities.GetDateAndTime());

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        Console.WriteLine("NoteArchiver.LogError: {0}", ex);
                        throw ex;
                    }

                }
            }
        }

        //Returns from sqlite the id of newly added record or -1 to indicate error
        /*
        public long AddNoteToDB(Note note)
        {
            if (note == null || string.IsNullOrEmpty(dbFullNameAndPath))
                return -1;

            long newRecordID = -1;

            //TODO: Set unique index on guid in table?
            //string insertSQL = string.Format("INSERT INTO notes(guid, tomboy_version, title, note_version, note_text, note_content, " +
                                             "create_date, last_change_date, notebook_name) VALUES('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, {8})",
                                             note.GUID, note.Version, note.Title, note.NoteVersion, note.Text, note.Content,
                                             note.CreateDate, note.ChangeDate, note.Notebook);
            

            //string insertSQL = CreateInsertSQL(note);

            //DEBUGGING here...
            //
            //if (note.Title == "Freelancing" || note.Title == "Debian 10 Install" || note.Title == "Amazon")
            //{
            //    System.Diagnostics.Debugger.Break();
            //}
            

            //newRecordID = AddNoteToDB(insertSQL, note.GUID);

            //return newRecordID;

        }
        */

        /*
        private long AddNoteToDB(string insertSQL, string guid)
        {
            //Console.WriteLine("\n*** NoteArchiver.AddNoteToDB ***");

            string dataSource = string.Format("Data Source={0}", this.dbFullNameAndPath);
            int rowsInserted = -1;
            long newRowID = -1;

            using (var connection = new SqliteConnection(dataSource))
            {
                connection.Open();

                var cmd = connection.CreateCommand();

                //command.CommandText = @"SELECT * FROM tomboy_notes";
                cmd.CommandText = insertSQL;

                try
                {
                    rowsInserted = cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("NoteArchiver.AddNoteToDB: Couldn't add note {0} to database...", guid);
                    Console.WriteLine("NoteArchiver.AddNoteToDB: insertSQL: {0}\n", insertSQL);
                    Console.WriteLine(ex);
                    System.Diagnostics.Debugger.Break();

                }

                //Console.WriteLine("NoteArchiver.AddNoteToDB: There was/were " + rowsInserted.ToString() + " row/rows inserted");

                //Let's see if we can get the last _id from sqlite
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT last_insert_rowid()";

                try
                {
                    newRowID = (long)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("AddNoteToDB: Exception occurred: {0}", ex);
                }

                //Console.WriteLine("AddNoteToDB: Row id from sqlite: {0}", newRowID.ToString());
                return newRowID;

            }// end using

        }// end method
        */

    }

}