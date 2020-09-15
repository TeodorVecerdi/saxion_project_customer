using UnityEngine;

[RequireComponent(typeof(ButtonEvents))]
public class InGameSettings : MonoBehaviour {
    private ButtonEvents buttonEvents;

    private void Awake() {
        buttonEvents = GetComponent<ButtonEvents>();
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Escape) && !GameTime.IsPaused) {
            buttonEvents.OnSettingsInGameClick();
        }
    }
}