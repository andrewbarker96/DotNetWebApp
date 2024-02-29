using System.Data.SQLite;

// ...

public class ConnectionManager
{
    public void ConnectToDatabase()
    {
        string connectionString = "Data Source=|DataDirectory|/database.db;Version=3;";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "select * from contact";
            using (var command = new SQLiteCommand(sql, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("Name: " + reader["Name"] + "\tScore: " + reader["Score"]);
                    }
                }
            }
        }
    }
}