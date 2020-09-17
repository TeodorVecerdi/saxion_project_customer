using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialTimeline : MonoBehaviour {
    public TMP_Text Years;

    public void SetYear(string year) {
        Years.text = year;
    }
}
