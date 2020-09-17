using System;
using System.Collections.Generic;
using MySqlConnector;
using UnityEngine;
using TMPro;

public class DBConnection : IDisposable {
    private MySqlConnection connection;

    public DBConnection() {
        connection = new MySqlConnection("server=freedb.tech;port=3306;user id=freedb_teodorvecerdi;password=ZZJfLS?!X72rN/bc;database=freedb_project_customer;sslmode=None");
        connection.Open();
    }

    public void DebugHighscores(int numScores) {
        Debug.Log($"[UUID]\t\t[NAME]\t\t[REASON]\t\t[SCORE]");
        using (var command = new MySqlCommand("SELECT * FROM highscore ORDER BY score DESC LIMIT @limit", connection)) {
            command.Parameters.AddWithValue("limit", numScores);

            //command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    Debug.Log($"{reader.GetString(1)}\t{reader.GetString(2)}\t{reader.GetFloat(3)}");
                }
            }
        }
    }

    public List<Highscore> Highscores(int numScores) {
        using (var command = new MySqlCommand("SELECT * FROM highscore ORDER BY score DESC LIMIT @limit", connection)) {
            command.Parameters.AddWithValue("limit", numScores);

            // var enq = command.ExecuteNonQuery();
            // Debug.Log(enq.ToString());
            var highscores = new List<Highscore>();
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    highscores.Add(new Highscore { Name = reader.GetString(1),  DeathReason = reader.GetString(2), Score = reader.GetFloat(3)});
                }
                return highscores;
            }
        }
    }

    public struct Highscore {
        public string Name;
        public string DeathReason;
        public float Score;
    }

    public void InsertHighScore(string username, string deathReason, float score) {
        using (var command = new MySqlCommand($"INSERT INTO highscore (UUID, name, death_reason, score)" + "VALUES(@guid,@name,@reason,@score)", connection)) {
            command.Parameters.AddWithValue("guid", Guid.NewGuid().ToString());
            command.Parameters.AddWithValue("name", username);
            command.Parameters.AddWithValue("reason", deathReason);
            command.Parameters.AddWithValue("score", score);
            command.ExecuteNonQuery();
        }
    }

    public void ClearTable() { }

    public void Dispose() {
        connection.Close();
    }
}