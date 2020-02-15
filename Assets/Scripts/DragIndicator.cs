using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIndicator : MonoBehaviour {
    public static DragIndicator instance;
    Renderer myRenderer;
    public bool EnableRenderer {
        get => myRenderer.enabled;
        set => myRenderer.enabled = value;
    }

    void Awake() {
        instance = this;
        myRenderer = GetComponent<Renderer>();
    }

    void Start() {
        EnableRenderer = false;
    }
}
