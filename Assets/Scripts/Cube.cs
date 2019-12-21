using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour {
    [SerializeField] Renderer capRenderer = null;

    TileInfo tileInfo;
    
    void Awake() {
        tileInfo = UserInterface.instance.InstantiateTileInfo(this);
    }

    void OnDestroy() {
        Destroy(tileInfo.gameObject);
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
        Destroy(gameObject);
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
}
