using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{
    public Vector3 offset = new Vector3(5, 5);

    private RectTransform thisBtn;

    void Start()
    {
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

    public void OnSettingsClick() {
        if (SceneManager.GetActiveScene().name == "Game") {
            CursorController.Instance.Default();
            GameTime.IsPaused = true;
        }
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
    }

    public void OnBackBtnClick()
    {
        if (GameTime.IsPaused) {
            CursorController.Instance.Game();            
            GameTime.IsPaused = false;
        }
        SceneManager.UnloadSceneAsync("SettingsMenu");
    }

    public void SubmitName(string turtleName)
    {
        Debug.Log(turtleName);
    }

    public void OnMainMenuClick()
    { 
        CursorController.Instance.Default();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void OnAboutUsClick()
    {
        //website link
    }

    public void OnSeeTurtlesClick()
    {
        //website link
    }

    public void OnLogoClick()
    {
        //website link
    }

    public void OnMouseEnter()
    {
        thisBtn.position += offset;
    }

    public void OnMouseExit()
    {
        thisBtn.position -= offset;
    }
}
