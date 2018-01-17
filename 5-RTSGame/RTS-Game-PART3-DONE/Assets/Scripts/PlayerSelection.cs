using UnityEngine;
using System.Collections;

public class PlayerSelection : MonoBehaviour {    
    // Selection Circle
    public GameObject circle;

    // OnSelect is called by the RTS Selection System
    void OnSelect() {
        Debug.Log("OnSelect");
        circle.SetActive(true);
    }
    
    // OnDeselect is called by the RTS Selection System
    void OnDeselect() {
        Debug.Log("OnDeselect");
        circle.SetActive(false);
    }
}
