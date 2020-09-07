using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{
    public Vector3 offset = new Vector3(5, 5);

    private RectTransform thisBtn;

    void Start()
    {
        thisBtn = GetComponent<RectTransform>();
    }

    public void OnStartGameClick()
    {
        SceneManager.LoadScene("WorldGen", LoadSceneMode.Single);
    }

    public void OnSettingsClick() {
        if (SceneManager.GetActiveScene().name == "WorldGen")
            GameTime.IsPaused = true;
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
    }

    public void OnBackBtnClick()
    {
        if (GameTime.IsPaused)
            GameTime.IsPaused = false;
        SceneManager.UnloadScene("SettingsMenu");
    }

    public void OnMainMenuClick()
    { 
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

    public void OnExitClick()
    {
        Application.Quit();
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
