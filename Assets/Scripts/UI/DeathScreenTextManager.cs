﻿using UnityEngine;
using TMPro;
using System;

public class DeathScreenTextManager : MonoBehaviour
{
    public TMP_Text currentScoreText;
    [Header("Change scenario number")]
    public int deathScenario = 1;
    public string reasonHunger = "has died of hunger. This is the common fate of the ocean turtle. Unless you have a say in the matter. Visit www.seeturtles.org to help fight in our cause.";
    public string reasonPlastic = "has died while chocking on a piece of plastic. A sad death for such a beautiful species. All of this happened because of the carelessness of the people. Help in our cause to reduce the waste in the ocean. Visit www.seeturtles.org to learn more.";
    public string reasonPoaching = "has been taken by poachers. Sadly you have died. You are not the first neither the last. This can be changed of course, you have a say in the matter as well. Help our cause by visiting www.seeturtles.org and learn more.";

    private string deathReason;

    private void Start()
    {
        string name = PlayerPrefs.GetString("name");
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
        currentScoreText.text = $"<b>" + name + "</b> " + deathReason;
    }
}
