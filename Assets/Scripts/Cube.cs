﻿using System;
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
        if (TileInfo.LevelUpPossible) {
            Network.instance.OnClientLevelUp(X, Y);
            Network.instance.sharedAudioSource.PlayOneShot(Network.instance.upgradeClip);
        } else {
            Network.instance.OnClientClick(X, Y);
        }
        
        Network.instance.sharedAudioSource.PlayOneShot(Network.instance.clickClip);
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
