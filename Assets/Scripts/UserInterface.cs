using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
    [SerializeField] Image playerImage = null;
    [SerializeField] PlayerInfoWindow playerInfoWindow = null;
    [SerializeField] ResultWindow resultWindow = null;
    [SerializeField] GameObject tileInfoPrefab = null;
    [SerializeField] Transform tileInfoGroup = null;

    public static UserInterface instance;

    internal TileInfo InstantiateTileInfo(Cube cube) {
        var tileInfo = Instantiate(tileInfoPrefab, tileInfoGroup).GetComponent<TileInfo>();
        tileInfo.Tile = cube.transform;
        tileInfo.InitBind();
        tileInfo.UpdatePosition();
        return tileInfo;
    }

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

    public void OnResultGameScore() {
        playerInfoWindow.gameObject.SetActive(false);
        resultWindow.gameObject.SetActive(true);

        resultWindow.ClearAll();
    }

    public void OnResultAddEntry(string color, string score) {
        resultWindow.AddEntry(color, score);
    }
}
