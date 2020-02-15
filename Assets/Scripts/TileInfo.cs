using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileInfo : MonoBehaviour {
    [SerializeField] Image image = null;

    [SerializeField] TextMeshProUGUI defenceText = null;
    [SerializeField] TextMeshProUGUI offenceText = null;
    [SerializeField] TextMeshProUGUI productivityText = null;
    [SerializeField] TextMeshProUGUI attackRangeText = null;

    Transform tile;
    RectTransform rectParent;
    MainCamera mainCam;
    InterfaceCamera interfaceCam;

    public Transform Tile {
        get => tile;
        set => tile = value;
    }

    public bool LevelUpPossible => false; //levelUpPossible.text.Equals("!");

    void Awake() {
        InitBind();
    }

    public void InitBind() {
        mainCam = Camera.main.GetComponent<MainCamera>();
        interfaceCam = GameObject.FindObjectOfType<InterfaceCamera>();
        rectParent = transform.parent.GetComponent<RectTransform>();
    }

    public void SetData(string color, string v, string l, string p, int defence, int offence, int productivity, int attackRange) {
        ColorUtility.TryParseHtmlString(color, out var c);
        image.color = c;
        offenceText.text = offence.ToString();
        defenceText.text = defence.ToString();
        productivityText.text = productivity.ToString();
        attackRangeText.text = attackRange.ToString();
        //levelUpPossible.text = p;
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
