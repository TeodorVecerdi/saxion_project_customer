using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetText : MonoBehaviour {
    public TMP_Text Target;
    private Slider slider;
    private void Awake() {
        slider = GetComponent<Slider>();
    }
    private void Update() {
        Target.text = $"{slider.value:.00}";
    }
}
