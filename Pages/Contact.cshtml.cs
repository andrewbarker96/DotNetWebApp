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

                //return new OkResult();
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                _logger.LogError($"Error: {ex.Message}");
                //return new StatusCodeResult(500); // Internal Server Error
            }
        }
    }
}
