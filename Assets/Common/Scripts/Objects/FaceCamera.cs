using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {
    // Properties
    public bool LockAxisX = false;
    public bool LockAxisY = false;
    public bool LockAxisZ = false;
    public bool IsFlipped = false; // this would face directly AWAY from the camera.
    private Vector3 originalLocalEulerAngles; // kind of a hack to solve cases where it flip-flops like crazy.
    private System.Action LookAtCamera;
    // References
    private Transform tf_camera;


    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    private void Awake() {
        if (LockAxisX || LockAxisY || LockAxisZ) LookAtCamera = LookAtCamera_LockedAxis;
        else LookAtCamera = LookAtCamera_NoLockedAxis;
        originalLocalEulerAngles = this.transform.localEulerAngles;
    }


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    void Update() {
        if (LookAtCamera == null) return; // Editor-print-errors-after-reload-scripts check.
        if (tf_camera == null) tf_camera = Camera.main.transform;
        LookAtCamera();
        if (!IsFlipped) // So, I really debated about what "flipped" means. For my personal convenience, I'm going to assume by DEFAULT we edit things with their forward pointing away from camera, and to look at camera, we want their forward pointing TOWARD the camera.
            this.transform.rotation = Quaternion.LookRotation(-this.transform.forward, this.transform.up);
        //transform.eulerAngles = transform.eulerAngles * -1;// Quaternion.Euler(0, 180, 0);
    }
    private void LookAtCamera_NoLockedAxis() {
        this.transform.LookAt(tf_camera);
    }
    private void LookAtCamera_LockedAxis() {
        this.transform.localEulerAngles = originalLocalEulerAngles;
        this.transform.rotation = Quaternion.LookRotation(-this.transform.forward, this.transform.up);//HACK TESTING!! (this is a HACK I've just added, and there's a chance it will cause no issues and actually just solve everything. We pray!)
        Vector3 currEulerAngles = this.transform.eulerAngles;

        this.transform.LookAt(tf_camera);

        if (LockAxisX) this.transform.eulerAngles = new Vector3(currEulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        if (LockAxisY) this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, currEulerAngles.y, this.transform.eulerAngles.z);
        if (LockAxisZ) this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, currEulerAngles.z);
    }
}
