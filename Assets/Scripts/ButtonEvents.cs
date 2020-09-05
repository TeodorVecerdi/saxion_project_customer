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

    public void OnSettingsClick()
    {
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Single);
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
