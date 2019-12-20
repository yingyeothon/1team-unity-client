using UnityEngine;

public class Cube : MonoBehaviour {
    void OnMouseEnter() {
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    }

    void OnMouseExit() {
        transform.localPosition = new Vector3(transform.localPosition.x, -0.4f, transform.localPosition.z);
    }
}
