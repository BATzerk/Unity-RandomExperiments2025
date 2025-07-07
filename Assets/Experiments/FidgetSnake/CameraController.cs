using UnityEngine;

public class CameraController : MonoBehaviour {
    // Properties
    private float aimSensitivity = 600f;
    //private float zoomSensitivity = 50f;
    private float distance = 6f;
    private float yaw = 0f;
    private float pitch = 0f;


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * aimSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * aimSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -89f, 89f);
        //distance += Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 direction = rotation * Vector3.forward;
        transform.position = -direction * distance;
        transform.LookAt(Vector3.zero);
    }
}
