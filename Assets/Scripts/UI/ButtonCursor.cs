using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCursor : MonoBehaviour {
    private CursorController instance;
    private void Start() {
        instance = FindObjectOfType<CursorController>();
        var eventTrigger = gameObject.AddComponent<EventTrigger>();
        
        var pointerEnter = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
        pointerEnter.callback.AddListener(arg0 => instance.Link());
        var pointerExit = new EventTrigger.Entry {eventID = EventTriggerType.PointerExit};
        pointerExit.callback.AddListener(arg0 => instance.Default());
        
        eventTrigger.triggers.Add(pointerEnter);
        eventTrigger.triggers.Add(pointerExit);
    }
}