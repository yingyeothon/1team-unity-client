using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour {
    [SerializeField] Image playerImage = null;
    [SerializeField] PlayerInfoWindow playerInfoWindow = null;
    [SerializeField] ResultWindow resultWindow = null;
    [SerializeField] GameObject tileInfoPrefab = null;
    [SerializeField] Transform tileInfoGroup = null;
    [SerializeField] Transform waitWindow = null;
    [SerializeField] TextMeshProUGUI waitText = null;
    [SerializeField] TextMeshProUGUI runningTimeText = null;

    public static UserInterface instance;

    Dictionary<System.Tuple<int, int>, Cube> cubeDict = new Dictionary<System.Tuple<int, int>, Cube>();
    private string playerColor;

    internal TileInfo InstantiateTileInfo(Cube cube) {
        var tileInfo = Instantiate(tileInfoPrefab, tileInfoGroup).GetComponent<TileInfo>();
        tileInfo.Tile = cube.transform;
        tileInfo.InitBind();
        tileInfo.UpdatePosition();

        cubeDict[new System.Tuple<int, int>(cube.X, cube.Y)] = cube;

        return tileInfo;
    }

    public UnityAction onResultWindowClose;
    public UnityAction onResultWindowRestart;

    public void Awake() {
        instance = this;

        //onResultWindowClose += TestFunc;
        onResultWindowRestart += RestartGame;
    }

    void TestFunc() {
        Debug.Log("Test Func");
    }

    public void OnPlayerColorChange(string playerColor) {
        this.playerColor = playerColor;

        ColorUtility.TryParseHtmlString(playerColor, out var color);
        playerImage.color = color;
    }

    public void OnTileChanges(Command.TileChange[] tileChanges) {
        Debug.Log("OnTileChanges");

        foreach (var tc in tileChanges) {
            var xy = new System.Tuple<int, int>(tc.x, tc.y);
            var cube = cubeDict[xy];
            cube.TileInfo.SetData(
                tc.color,
                tc.v.ToString(),
                tc.l.ToString(),
                tc.color.Equals(playerColor) && tc.p ? "!" : ""
            );
            cube.SetColor(tc.color);
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

    public void OnWait(int age) {
        waitWindow.gameObject.SetActive(true);
        resultWindow.gameObject.SetActive(false);
        waitText.text = $"Please Wait...{age}";
    }

    public void OnRunning(int age) {
        waitWindow.gameObject.SetActive(false);
        resultWindow.gameObject.SetActive(false);
        runningTimeText.text = $"{age} sec";
    }

    private static void RestartGame() {
        var loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }
}
