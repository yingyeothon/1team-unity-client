using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour {
    [SerializeField] Renderer capRenderer = null;
    [SerializeField] int x = 0;
    [SerializeField] int y = 0;
    [SerializeField] Renderer cubeRenderer = null;

    TileInfo tileInfo;

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public TileInfo TileInfo => tileInfo;

    public bool IsMine => UserInterface.instance.IsMine(this);

    void Awake() {
        tileInfo = UserInterface.instance.InstantiateTileInfo(this);

    }

    void OnDestroy() {
        if (tileInfo != null) {
            Destroy(tileInfo.gameObject);
        }
    }

    void OnMouseEnter() {
        SetHighlightHeightConditional();
    }

    void OnMouseOver() {
        SetHighlightHeightConditional();
        if (LastOverCube != this) {
            Debug.Log($"Over: {this}");
            LastOverCube = this;
        }
    }

    void OnMouseExit() {
        SetDefaultHeight();
    }

    void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        Debug.Log($"OnMouseDown(): {this}");
        if (Network.instance != null) {
            if (TileInfo.LevelUpPossible) {
                Network.instance.OnClientLevelUp(X, Y);
                Network.instance.sharedAudioSource.PlayOneShot(Network.instance.upgradeClip);
            } else {
                Network.instance.OnClientClick(X, Y);
            }

            Network.instance.sharedAudioSource.PlayOneShot(Network.instance.clickClip);
        }
    }

    void OnMouseUp() {
        Debug.Log($"OnMouseUp(): {this}");
        DragIndicator.instance.EnableRenderer = false;

        if (this != LastOverCube) {
            Debug.Log($"Drag: {this} --> {LastOverCube}");
            //Network.instance.
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        


        //if (IsMine) {
        UserInterface.instance.OpenPurchaseWindow(this);
        //}
    }

    void OnMouseDrag() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        //Debug.Log($"OnMouseDrag(): {this}");

        var ray = MainCamera.instance.Cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        var plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out var distance)) {
            var point = ray.GetPoint(distance);
            DragIndicator.instance.transform.position = point;
            DragIndicator.instance.EnableRenderer = true;
        }
    }

    private void SetHighlightHeightConditional() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            SetDefaultHeight();
            return;
        }
        SetHighlightHeight();
    }

    void SetHighlightHeight() {
        capRenderer.enabled = true;
        //transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    }

    void SetDefaultHeight() {
        capRenderer.enabled = false;
        //transform.localPosition = new Vector3(transform.localPosition.x, -0.4f, transform.localPosition.z);
    }

    internal void SetColor(string v) {
        ColorUtility.TryParseHtmlString(v, out var color);
        Color = color;
    }

    public Color Color {
        get => cubeRenderer.material.GetColor("_Color");
        set => cubeRenderer.material.SetColor("_Color", value);
    }
    static public Cube LastOverCube { get; private set; }
}
