using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MainCamera : MonoBehaviour {
    void Update() {
        transform.LookAt(Vector3.zero);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -transform.localPosition.x);
    }
}
