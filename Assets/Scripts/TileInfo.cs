using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileInfo : MonoBehaviour {
    [SerializeField] Image image = null;
    [SerializeField] TextMeshProUGUI value = null;
    [SerializeField] TextMeshProUGUI level = null;
    [SerializeField] TextMeshProUGUI levelUpPossible = null;

    Transform tile;
    RectTransform rectParent;
    MainCamera mainCam;
    InterfaceCamera interfaceCam;

    public Transform Tile {
        get => tile;
        set => tile = value;
    }

    public bool LevelUpPossible => levelUpPossible.text.Equals("!");

    void Awake() {
        InitBind();
    }

    public void InitBind() {
        mainCam = Camera.main.GetComponent<MainCamera>();
        interfaceCam = GameObject.FindObjectOfType<InterfaceCamera>();
        rectParent = transform.parent.GetComponent<RectTransform>();
    }

    public void SetData(string color, string v, string l, string p) {
        ColorUtility.TryParseHtmlString(color, out var c);
        image.color = c;
        value.text = v;
        level.text = l;
        levelUpPossible.text = p;
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
