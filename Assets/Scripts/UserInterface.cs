using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class UserInterface : MonoBehaviour {
    [SerializeField] Image playerImage = null;
    [SerializeField] PlayerInfoWindow playerInfoWindow = null;
    [SerializeField] ResultWindow resultWindow = null;
    [SerializeField] GameObject tileInfoPrefab = null;
    [SerializeField] Transform tileInfoGroup = null;
    [SerializeField] Transform waitWindow = null;
    [SerializeField] TextMeshProUGUI waitText = null;
    [SerializeField] TextMeshProUGUI runningTimeText = null;
    [SerializeField] TextMeshProUGUI myCellCount = null;
    [SerializeField] TextMeshProUGUI yourCellCount = null;
    [SerializeField] GameObject upgradeWindow = null;
    [SerializeField] GameObject purchaseWindow = null;
    [SerializeField] TextMeshProUGUI energyText = null;
    //[SerializeField] RectTransform upgradeTargetCubeFrame = null;
    //[SerializeField] RectTransform purchaseTargetCubeFrame = null;
    //[SerializeField] Camera interfaceCam = null;

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

    internal void OpenPurchaseWindow(Cube cube) {
        Debug.Log("OpenPurchaseWindow()");
        lastSelectedCube = cube;
        purchaseWindow.gameObject.SetActive(true);
    }

    internal void OpenUpgradeWindow(Cube cube) {
        Debug.Log("OpenUpgradeWindow()");
        lastSelectedCube = cube;
        upgradeWindow.gameObject.SetActive(true);
    }

    public UnityAction onResultWindowRestart;
    private Cube lastSelectedCube;

    public string MyCellCountText {
        get => myCellCount.text;
        set => myCellCount.text = value;
    }
    public string YourCellCountText {
        get => yourCellCount.text;
        set => yourCellCount.text = value;
    }

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

    public void OnEnergyChange(int value) {
        energyText.text = value.ToString();
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
                tc.color.Equals(playerColor) && tc.p ? "!" : "",
                tc.defence,
                tc.offence,
                tc.productivity,
                tc.attackRange
            );
            cube.SetColor(tc.color);
        }

        var cubeCountByColor = cubeDict.GroupBy(e => e.Value.Color).ToDictionary(g => g.Key, g => g.Count());
        ColorUtility.TryParseHtmlString(playerColor, out var playerColorColor);
        if (cubeCountByColor.TryGetValue(playerColorColor, out var playerColorCount)) {
            MyCellCountText = playerColorCount.ToString();  
        }
        var yourColorCount = cubeCountByColor.FirstOrDefault(e => e.Key != Color.white && e.Key != Color.black && e.Key != playerColorColor);
        YourCellCountText = yourColorCount.Value.ToString();
    }

    public void OnResultGameScore() {
        playerInfoWindow.gameObject.SetActive(false);
        resultWindow.gameObject.SetActive(true);
        resultWindow.ClearAll();
    }

    public void OnResultAddEntry(string color, string score) {
        resultWindow.AddEntry(color, score);
    }

    public void OnLoggingIn() {
        waitWindow.gameObject.SetActive(true);
        resultWindow.gameObject.SetActive(false);
        waitText.text = $"Logging in...";
    }

    public void OnMatching() {
        waitWindow.gameObject.SetActive(true);
        resultWindow.gameObject.SetActive(false);
        waitText.text = $"Matching...";
    }

    public void OnWait(int age) {
        waitWindow.gameObject.SetActive(true);
        resultWindow.gameObject.SetActive(false);
        waitText.text = $"Waiting other players...{age}";
    }

    public void OnRunning(int age) {
        waitWindow.gameObject.SetActive(false);
        resultWindow.gameObject.SetActive(false);
        runningTimeText.text = $"{age} sec";
    }

    public static void RestartGame() {
        var loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public void UpgradeOffence() {
        var x = lastSelectedCube.X;
        var y = lastSelectedCube.Y;
        Network.instance.UpgradeOffence(x, y);
        upgradeWindow.gameObject.SetActive(false);
        Network.instance.sharedAudioSource.PlayOneShot(Network.instance.upgradeClip);
    }

    public void UpgradeDefence() {
        var x = lastSelectedCube.X;
        var y = lastSelectedCube.Y;
        Network.instance.UpgradeDefence(x, y);
        upgradeWindow.gameObject.SetActive(false);
        Network.instance.sharedAudioSource.PlayOneShot(Network.instance.upgradeClip);
    }

    public void UpgradeProductivity() {
        var x = lastSelectedCube.X;
        var y = lastSelectedCube.Y;
        Network.instance.UpgradeProductivity(x, y);
        upgradeWindow.gameObject.SetActive(false);
        Network.instance.sharedAudioSource.PlayOneShot(Network.instance.upgradeClip);
    }

    public void UpgradeAttackRange() {
        var x = lastSelectedCube.X;
        var y = lastSelectedCube.Y;
        Network.instance.UpgradeAttackRange(x, y);
        upgradeWindow.gameObject.SetActive(false);
        Network.instance.sharedAudioSource.PlayOneShot(Network.instance.upgradeClip);
    }

    public void ConquerCell() {
        var x = lastSelectedCube.X;
        var y = lastSelectedCube.Y;
        Network.instance.ConquerCell(x, y);
        purchaseWindow.gameObject.SetActive(false);
        Network.instance.sharedAudioSource.PlayOneShot(Network.instance.upgradeClip);
    }

    public void CancelUpgrade() {
        upgradeWindow.gameObject.SetActive(false);
    }

    public void CancelPurchase() {
        purchaseWindow.gameObject.SetActive(false);
    }

    internal bool IsMine(Cube cube) {
        if (ColorUtility.TryParseHtmlString(playerColor, out var color)) {
            return color == cube.Color;
        } else {
            return false;
        }
    }
}
