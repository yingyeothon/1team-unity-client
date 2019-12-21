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
    }

    void OnMouseExit() {
        SetDefaultHeight();
    }

    void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (TileInfo.Level == 0 && TileInfo.Value >= 10) {
            Network.instance.OnClientLevelUp(X, Y);
        } else if (TileInfo.Level == 1 && TileInfo.Value >= 25) {
            Network.instance.OnClientLevelUp(X, Y);
        } else if (TileInfo.Level == 2 && TileInfo.Value >= 35) {
            Network.instance.OnClientLevelUp(X, Y);
        } else if (TileInfo.Level == 3 && TileInfo.Value >= 40) {
            Network.instance.OnClientLevelUp(X, Y);
        } else if (TileInfo.Level == 4 && TileInfo.Value >= 50) {
            Network.instance.OnClientLevelUp(X, Y);
        } else {
            Network.instance.OnClientClick(X, Y);
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
        cubeRenderer.material.SetColor("_Color", color);
    }
}
