using ASM.Entity;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Data
{
    class ContactModel
    {

        public static void AddData(string inputText)
        {
            //using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            //{
            //    db.Open();

            //    SqliteCommand insertCommand = new SqliteCommand();
            //    insertCommand.Connection = db;

            //    // Use parameterized query to prevent SQL injection attacks
            //    insertCommand.CommandText = "INSERT INTO MyTable VALUES (@name, @email, @phone);";
            //    insertCommand.Parameters.AddWithValue("@name", "minhcak" );
            //    insertCommand.Parameters.AddWithValue("@email", "minhcak");

            //    insertCommand.ExecuteReader();

            //    db.Close();
            //}

        }
    }
}
