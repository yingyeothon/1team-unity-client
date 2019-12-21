using UnityEngine;

public class InterfaceCamera : MonoBehaviour {

    Camera interfaceCamera;
    public Camera Cam => interfaceCamera;

    void Awake() {
        interfaceCamera = GetComponent<Camera>();
    }
}
