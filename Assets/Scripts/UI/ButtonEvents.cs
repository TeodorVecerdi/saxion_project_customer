using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour {
    public Vector3 offset = new Vector3(5, 5);
    public List<GameObject> InGameSettingsItemsToggle;
    public Transform SunShaftsContainer;
    private static List<GameObject> inGameSettingsItemsToggleCarryover;
    private static Transform sunShaftsCarryover;
    private RectTransform thisBtn;

    private void Start() {
        thisBtn = GetComponent<RectTransform>();
    }

    public void OnStartGameClick() {
        CursorController.Instance.Default();
        PlayerPrefs.SetString("name", "Bubbles");
        PlayerPrefs.Save();
        SceneManager.LoadScene("NameMenu", LoadSceneMode.Single);
    }

    public void OnRetryClick() {
        CursorController.Instance.Default();
        SceneManager.LoadScene("NameMenu", LoadSceneMode.Single);
    }

    public void OnPlayClick(TMP_InputField inputField) {
        CursorController.Instance.Default();
        GameTime.IsPaused = false;
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
    }

    public void OnSettingsInGameClick() {
        CursorController.Instance.Default();
        GameTime.IsPaused = true;
        SunShaftsContainer.position = Vector3.left * 1000f;
        InGameSettingsItemsToggle.ForEach(obj => obj.SetActive(false));
        inGameSettingsItemsToggleCarryover = new List<GameObject>();
        inGameSettingsItemsToggleCarryover.AddRange(InGameSettingsItemsToggle);
        sunShaftsCarryover = SunShaftsContainer;
        SceneManager.LoadScene("InGameSettingsMenu", LoadSceneMode.Additive);
        
        SoundManager.PauseSound("theme_game");
        SoundManager.PlaySound("theme_menu", skipIfAlreadyPlaying: true, resumeIfPaused: true);
    }

    public void OnSettingsClick() {
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Single);
    }

    public void OnResumeClick() {
        CursorController.Instance.Game();
        GameTime.IsPaused = false;
        inGameSettingsItemsToggleCarryover.ForEach(obj => obj.SetActive(true));
        sunShaftsCarryover.position = Vector3.zero;
        SceneManager.UnloadSceneAsync("InGameSettingsMenu");
        
        SoundManager.PauseSound("theme_menu");
        SoundManager.PlaySound("theme_game", skipIfAlreadyPlaying: true, resumeIfPaused: true);
    }

    public void OnBackBtnClick() {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void OnMainMenuClick() {
        CursorController.Instance.Default();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        
        
        SoundManager.StopSound("theme_game");
        SoundManager.PlaySound("theme_menu", skipIfAlreadyPlaying: true, resumeIfPaused: true);
    }

    public void OnMouseEnter() {
        thisBtn.position += offset;
    }

    public void OnMouseExit() {
        thisBtn.position -= offset;
    }
}