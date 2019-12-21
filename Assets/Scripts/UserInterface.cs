using System.Collections.Generic;
using Response;
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

    Dictionary<System.Tuple<int, int>, Cube> cubeDict = new Dictionary<System.Tuple<int, int>, Cube>();

    internal TileInfo InstantiateTileInfo(Cube cube) {
        var tileInfo = Instantiate(tileInfoPrefab, tileInfoGroup).GetComponent<TileInfo>();
        tileInfo.Tile = cube.transform;
        tileInfo.InitBind();
        tileInfo.UpdatePosition();

        cubeDict[new System.Tuple<int, int>(cube.X, cube.Y)] = cube;

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

    public void OnTileChanges(TileChange[] tileChanges) {
        Debug.Log("OnTileChanges");

        foreach (var tc in tileChanges) {
            var xy = new System.Tuple<int, int>(tc.x, tc.y);
            cubeDict[xy].TileInfo.SetData("#ff0000", tc.v.ToString(), tc.l.ToString());
        }
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
