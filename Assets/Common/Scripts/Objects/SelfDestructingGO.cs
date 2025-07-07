using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructingGO : MonoBehaviour
{
    // Properties
    [SerializeField] public float DestroyDelay = 5f;

    void Awake() {
        if (DestroyDelay >= 0)
            Destroy(this.gameObject, DestroyDelay);
        else
            Destroy(this.gameObject);
    }
}
