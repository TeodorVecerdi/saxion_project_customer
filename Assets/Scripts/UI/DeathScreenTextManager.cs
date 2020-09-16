using UnityEngine;
using TMPro;
using System;

public class DeathScreenTextManager : MonoBehaviour
{
    public TMP_Text deathText;
    [Header("Change scenario number")]
    [Multiline] public string reasonHunger = "has died of hunger. This is the common fate of the ocean turtle. Unless you have a say in the matter. Visit www.seeturtles.org to help fight in our cause.";
    [Multiline] public string reasonPlastic = "has died while chocking on a piece of plastic. A sad death for such a beautiful species. All of this happened because of the carelessness of the people. Help in our cause to reduce the waste in the ocean. Visit www.seeturtles.org to learn more.";
    [Multiline] public string reasonPoaching = "has been taken by poachers. Sadly you have died. You are not the first neither the last. This can be changed of course, you have a say in the matter as well. Help our cause by visiting www.seeturtles.org and learn more.";

    private string deathReason;

    private void Start() {
        // Reset cursor
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        
        string name = PlayerPrefs.GetString("name");
        int deathScenario = PlayerPrefs.GetInt("deathScenario", 1);
        switch (deathScenario) { 
            case 1: {
                deathReason = reasonHunger;
                break;
            }
            case 2: {
                deathReason = reasonPlastic;
                break;
            }
            case 3: {
                deathReason = reasonPoaching;
                break;
            }
        }
        deathText.text = $"<b>" + name + "</b> " + deathReason;
    }
}
