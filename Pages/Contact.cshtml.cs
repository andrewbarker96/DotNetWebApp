using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SQLite;

namespace DotNetWebApp.Pages
{
    public class ContactModel : PageModel
    {
        private readonly ILogger<ContactModel> _logger;

        public ContactModel(ILogger<ContactModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            string connectionString = "Data Source=|DataDirectory|/database.db;";

            try
            {
                string? name = Request.Form["name"];
                string? email = Request.Form["email"];
                string? message = Request.Form["message"];

                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                // Create the Contact table if it doesn't exist
                using (var createTableCommand = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Contact (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Email TEXT, Message TEXT)", connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }

                string sql = "INSERT INTO Contact (Name, Email, Message) VALUES (@name, @email, @message)";

                using var command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@message", message);

                command.ExecuteNonQuery();

                // Don't forget to close the connection... Better to define in a using tag
                // so it doesn't have to be manually closed as shown in getAllContacts().
                connection.Close();

                getAllContacts();
                //return new OkResult();
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                _logger.LogError($"Error: {ex.Message}");
                //return new StatusCodeResult(500); // Internal Server Error
            }
        }

        private void getAllContacts()
        {
            string connectionString = "Data Source=|DataDirectory|/database.db;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Contact";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Access data using reader["ColumnName"]
                            int id = Convert.ToInt32(reader["Id"]);
                            string? name = reader["Name"].ToString();
                            string? email = reader["Email"].ToString();
                            string? message = reader["Message"].ToString();

                            Console.WriteLine($"Id: {id}\n" +
                                $"Name: {name}\n" +
                                $"Email: {email}\n" +
                                $"Message: {message}\n");
                        }
                    }
                }
            }

        }
    }
}
