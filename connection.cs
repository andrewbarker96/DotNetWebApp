using Microsoft.Data.Sqlite;

// ...

public class ConnectionManager
{
    public void ConnectToDatabase()
    {
        string connectionString = "Data Source=./database.db;Version=3;";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "select * from contact";
            using (var command = new SqliteCommand(sql, connection))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
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