using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour {
    public Vector3 offset = new Vector3(5, 5);
    public List<GameObject> InGameSettingsItemsToggle;
    private static List<GameObject> inGameSettingsItemsToggleCarryover;
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
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void OnSettingsInGameClick() {
        CursorController.Instance.Default();
        GameTime.IsPaused = true;
        InGameSettingsItemsToggle.ForEach(obj => obj.SetActive(false));
        inGameSettingsItemsToggleCarryover = new List<GameObject>();
        inGameSettingsItemsToggleCarryover.AddRange(InGameSettingsItemsToggle);
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
    }

    public void OnSettingsClick() {
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Single);
    }

    public void OnBackBtnClick() {
        if (GameTime.IsPaused) {
            CursorController.Instance.Game();
            GameTime.IsPaused = false;
            inGameSettingsItemsToggleCarryover.ForEach(obj => obj.SetActive(true));
            SceneManager.UnloadSceneAsync("SettingsMenu"); 
        } else {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    public void OnMainMenuClick() {
        CursorController.Instance.Default();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void OnAboutUsClick() {
        //website link
    }

    public void OnMouseEnter() {
        thisBtn.position += offset;
    }

    public void OnMouseExit() {
        thisBtn.position -= offset;
    }
}