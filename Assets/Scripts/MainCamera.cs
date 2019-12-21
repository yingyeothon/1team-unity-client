using UnityEngine;

[ExecuteAlways]
public class MainCamera : MonoBehaviour {

    Camera mainCamera;
    public Camera Cam => mainCamera;

    void Awake() {
        mainCamera = GetComponent<Camera>();
    }

    void Update() {
        transform.LookAt(Vector3.zero);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -transform.localPosition.x);
    }
}
