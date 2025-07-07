using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGOToggler : MonoBehaviour
{
    // Properties
    private bool isGOsActive = true;
    // References
    [SerializeField] private List<GameObject> gosToToggle = new List<GameObject>();


    public void ToggleGOsActive() {
        isGOsActive = !isGOsActive;
        foreach (GameObject go in gosToToggle) {
            go.SetActive(isGOsActive);
        }
    }
}
