using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {
    private void Start() {
        SoundManager.StopSound("theme_game");
        SoundManager.PlaySound("theme_menu", skipIfAlreadyPlaying: true);
    }
}