using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour {
    public TMP_Text Name;
    public TMP_Text DeathReason;
    public TMP_Text Score;

    public void Build(string name, string deathReason, string score) {
        Name.text = name;
        DeathReason.text = deathReason;
        Score.text = score;
    }
}
