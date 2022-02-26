using System;
using Microsoft.Data.Sqlite;

namespace NotesApp
{
    public class NotesAppTester
    {

        public void Test() { }

        /* public static void Test(string dbName)
        {

            Console.WriteLine("\n*** NoteArchiver.Test ***");

            string dataSource = string.Format("Data Source={0}", dbName);

            using (var connection = new SqliteConnection(dataSource))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = @"SELECT * FROM tomboy_notes";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader.GetString(0);
                        Console.WriteLine("id: {0}", name);
                    }
                }
            }*/



    }

}