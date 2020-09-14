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

    }

    public void Dispose() {
        connection.Close();
    }
}