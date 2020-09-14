using System;
using MySqlConnector;
using UnityEngine;

public class DBConnection : IDisposable {
    private MySqlConnection connection;

    public DBConnection() {
        connection = new MySqlConnection("server=freedb.tech;port=3306;user id=freedb_teodorvecerdi;password=ZZJfLS?!X72rN/bc;database=freedb_project_customer;sslmode=None");
        connection.Open();
    }
    public void GetHighscores(int numScores) {
            /*using (var command = new MySqlCommand("SELECT * FROM turtlehighscores;", connection))
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    Debug.Log(reader.GetString(0));*/

            Debug.Log($"[UUID]\t\t[NAME]\t\t[TURTLE_NAME]\t\t[SCORE]");
            using (var command = new MySqlCommand("SELECT * FROM highscore ORDER BY score DESC LIMIT @limit", connection)) {
                command.Parameters.AddWithValue("limit", numScores);
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Debug.Log($"{reader.GetString(0)}\t{reader.GetString(1)}\t{reader.GetString(2)}\t{reader.GetFloat(3)}");
                    }
                }
            }

            /*using (var command = new MySqlCommand($"INSERT INTO highscore (UUID, name, death_reason, score)" + "VALUES(@guid,@name,@reason,@score)")) {
                command.Parameters.AddWithValue("guid", Guid.NewGuid().ToString());
                command.Parameters.AddWithValue("name", username);
                command.Parameters.AddWithValue("reason", deathReason);
                command.Parameters.AddWithValue("score", highscore);
                // ..... bla bla bla
            }*/

    }

    public void InsertHighScores(string username, string deathReason, float highscore)
    {
        using (var command = new MySqlCommand($"INSERT INTO highscore (UUID, name, death_reason, score)" + "VALUES(@guid,@name,@reason,@score)"))
        {
            command.Parameters.AddWithValue("guid", Guid.NewGuid().ToString());
            command.Parameters.AddWithValue("name", username);
            command.Parameters.AddWithValue("reason", deathReason);
            command.Parameters.AddWithValue("score", highscore);
            // ..... bla bla bla
        }
    }

    public void Dispose() {
        connection.Close();
    }
}