using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class Leaderboard : MonoBehaviour
{
    private TMP_Text text;

    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        var connection = new DBConnection();
        string name = PlayerPrefs.GetString("name");
        string deathScenario = PlayerPrefs.GetString("deathScenario");
        float currentScore = PlayerPrefs.GetFloat("CurrentScore");
        Debug.Log(currentScore);
        connection.InsertHighScore(name, deathScenario, currentScore);
        connection.PrintHighscores(5);
        text.text = connection.GetHighscores(5);
        
    }

}