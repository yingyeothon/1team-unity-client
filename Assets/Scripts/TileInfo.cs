using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileInfo : MonoBehaviour {
    [SerializeField] Image image = null;
    [SerializeField] TextMeshProUGUI value = null;
    [SerializeField] TextMeshProUGUI level = null;
    [SerializeField] Transform tile = null;
    [SerializeField] RectTransform rectParent = null;

    MainCamera mainCam;
    InterfaceCamera interfaceCam;

    public Transform Tile {
        get => tile;
        set => tile = value;
    }

    void Awake() {
        InitBind();
    }

    public void InitBind() {
        mainCam = Camera.main.GetComponent<MainCamera>();
        interfaceCam = GameObject.FindObjectOfType<InterfaceCamera>();
        rectParent = transform.parent.GetComponent<RectTransform>();
    }
    
    public void SetData(string color, string v, string l) {
        ColorUtility.TryParseHtmlString(color, out var c);
        image.color = c;
        value.text = v;
        level.text = l;
    }

    void LateUpdate() {
        UpdatePosition();
    }

    public void UpdatePosition() {
        if (tile != null) {
            var screenPoint = RectTransformUtility.WorldToScreenPoint(mainCam.Cam, tile.position + Vector3.up * 0.4f);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPoint, interfaceCam.Cam, out var localPoint);
            transform.localPosition = localPoint;
        }
    }
}
