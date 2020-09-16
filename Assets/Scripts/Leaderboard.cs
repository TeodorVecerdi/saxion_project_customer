using System;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class Leaderboard : MonoBehaviour {
    public RectTransform LeaderboardContainer;
    public LeaderboardEntry LeaderboardEntryPrefab;

    private void Start() {
        var connection = new DBConnection();
        string name = PlayerPrefs.GetString("name");
        string deathScenario = PlayerPrefs.GetString("deathScenarioText");
        float currentScore = PlayerPrefs.GetFloat("CurrentScore");
        connection.InsertHighScore(name, deathScenario, currentScore);
        foreach (var highscore in connection.Highscores(5)) {
            var prefab = Instantiate(LeaderboardEntryPrefab, LeaderboardContainer);
            prefab.Build(highscore.Name, highscore.DeathReason, $"{Math.Round(highscore.Score, 2):0.00}");
        }
    }
}