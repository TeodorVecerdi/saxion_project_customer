using System;
using MySqlConnector;
using UnityEngine;

public class DBConnection : IDisposable {
    private MySqlConnection connection;

    public DBConnection() {
        connection = new MySqlConnection("server=freedb.tech;port=3306;user id=freedb_teodorvecerdi;password=ZZJfLS?!X72rN/bc;database=freedb_project_customer;sslmode=None");
        connection.Open();
    }
    public string GetHighscores(int numScores) {
            Debug.Log($"[UUID]\t\t[NAME]\t\t[REASON]\t\t[SCORE]");
            using (var command = new MySqlCommand("SELECT * FROM highscore ORDER BY score DESC LIMIT @limit", connection)) {
                command.Parameters.AddWithValue("limit", numScores);
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        return ($"{reader.GetString(1)}\t{reader.GetString(2)}\t{reader.GetFloat(3)}");
                    }
                }
            return null;
            }
    }

    public void PrintHighscores(int numScores)
    {
        Debug.Log($"[UUID]\t\t[NAME]\t\t[REASON]\t\t[SCORE]");
        using (var command = new MySqlCommand("SELECT * FROM highscore ORDER BY score DESC LIMIT @limit", connection))
        {
            command.Parameters.AddWithValue("limit", numScores);
           // var enq = command.ExecuteNonQuery();
           // Debug.Log(enq.ToString());
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Debug.Log($"{reader.GetString(0)}\t{reader.GetString(1)}\t{reader.GetString(2)}\t{reader.GetFloat(3)}");
                }
            }
        }

    }

    public void InsertHighScore(string username, string deathReason, float score)
    {
        using (var command = new MySqlCommand($"INSERT INTO highscore (UUID, name, death_reason, score)" + "VALUES(@guid,@name,@reason,@score)", connection))
        {
            command.Parameters.AddWithValue("guid", Guid.NewGuid().ToString());
            command.Parameters.AddWithValue("name", username);
            command.Parameters.AddWithValue("reason", deathReason);
            command.Parameters.AddWithValue("score", score);
            command.ExecuteNonQuery();
        }
        
    }

    public void Dispose() {
        connection.Close();
    }
}