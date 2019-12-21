using Response;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
    [SerializeField] Image playerImage = null;
    [SerializeField] PlayerInfoWindow playerInfoWindow = null;
    [SerializeField] ResultWindow resultWindow = null;

    public static UserInterface instance;

    public UnityAction onResultWindowClose;

    public void Awake() {
        instance = this;

        //onResultWindowClose += TestFunc;
    }

    void TestFunc() {
        Debug.Log("Test Func");
    }
    
    public void OnPlayerColorChange(string playerColor) {
        ColorUtility.TryParseHtmlString(playerColor, out var color);
        playerImage.color = color;
    }

    public void OnTileChanges(TileChange[] tileChanges) {
        Debug.Log("OnTileChanges");
    }

    public void OnResultGameScore() {
        playerInfoWindow.gameObject.SetActive(false);
        resultWindow.gameObject.SetActive(true);

        resultWindow.ClearAll();
    }

    public void OnResultAddEntry(string color, string score) {
        resultWindow.AddEntry(color, score);
    }
}
