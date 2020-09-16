using UnityEngine;
using TMPro;
using System;

public class Highscore : MonoBehaviour
{
    public TMP_Text currentScoreText;
    public TMP_Text highScoreText;

    private float currentScore;
    private float highScore = 0;

    private void Start() {
        OnDeath();
    }

    private void OnDeath() {
        currentScore = PlayerPrefs.GetFloat("CurrentScore");
        highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        if (currentScore >= highScore) {
            highScore = currentScore;
        }
        ShowScores();
        SaveHighScore();
    }

    private void ShowScores() {
        currentScoreText.text = $"Score: <b>{Math.Round(currentScore, 2):0.00} years alive</b>";
        highScoreText.text = $"Personal Best: <b>{Math.Round(highScore, 2):0.00} years alive</b>";
    }

    private void SaveHighScore() {
        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.Save();
    }

}

