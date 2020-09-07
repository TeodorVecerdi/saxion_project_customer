using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Image HealthBar;
    public Image HungerBar;
    
    private void Update() {
        HealthBar.fillAmount = TurtleHealth.Instance.Health / TurtleHealth.Instance.MaxHealth;
        HungerBar.fillAmount = TurtleHealth.Instance.Hunger / TurtleHealth.Instance.MaxHunger;
    }
}