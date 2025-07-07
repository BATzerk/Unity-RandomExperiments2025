using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersionNumberText : MonoBehaviour {
    void Start() {
        string str = "v" + Application.version;

        TextMeshPro text = GetComponent<TextMeshPro>();
        if (text != null) { text.text = str; }
        else {
            TextMeshProUGUI textUGUI = GetComponent<TextMeshProUGUI>();
            if (textUGUI != null) { textUGUI.text = str; }
            else {
                Debug.LogWarning("Oops! A VersionNumberText doesn't have a TextMeshPro/TextMeshProUGUI component. GameObject: " + this.name);
            }
        }
    }
}
