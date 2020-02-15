using System.Collections.Generic;
using System.Linq;
using BestHTTP.WebSocket;
using Newtonsoft.Json;
using UnityEngine;

public class Network : MonoBehaviour {
    WebSocket webSocket;

    private Dictionary<int, Response.User> users = new Dictionary<int, Response.User>();

    public static Network instance;

    public Response.MatchInfo MatchInfo { get; set; }
    // = new Response.MatchInfo {
    //     url = "ws://localhost:3001",
    //     gameId = "local-test-1",
    //     playerId = "me",
    // };

    public AudioClip clickClip;
    public AudioClip upgradeClip;
    public AudioClip endClip;
    public AudioSource sharedAudioSource;

    public GameObject flameGroup;

    void Awake() {
        instance = this;
    }

    void Start() {
        webSocket = new WebSocket(
            new System.Uri($"{MatchInfo.url}?x-game-id={MatchInfo.gameId}&x-member-id={MatchInfo.playerId}"));
        webSocket.OnOpen += OnWebSocketOpen;
        webSocket.OnMessage += OnMessageReceived;
        webSocket.OnBinary += OnBinaryMessageReceived;
        webSocket.OnClosed += OnWebSocketClosed;
        webSocket.OnError += OnError;
        webSocket.Open();
    }

    void OnError(WebSocket ws, string error) {
        Debug.Log("Error: " + error);

        UserInterface.RestartGame();
    }

    private void OnWebSocketClosed(WebSocket webSocket, System.UInt16 code, string message) {
        Debug.Log("WebSocket Closed!");
    }

    private void OnBinaryMessageReceived(WebSocket webSocket, byte[] message) {
        Debug.Log("Binary Message received from server. Length: " + message.Length);
    }

    private void OnMessageReceived(WebSocket webSocket, string message) {
        Debug.Log("Text Message received from server: " + message);
        if (message.StartsWith("{\"type\":\"load\",")) {
            var loadResponse = JsonConvert.DeserializeObject<Response.LoadResponse>(message);

            foreach (var user in loadResponse.users) {
                users[user.index] = user;
            }

            UserInterface.instance.OnPlayerColorChange(loadResponse.me.color);
        }
        else if (message.StartsWith("{\"type\":\"enter\",")) {
            var enterResponse = JsonConvert.DeserializeObject<Response.EnterResponse>(message);

            users[enterResponse.newbie.index] = enterResponse.newbie;
        }
        else if (message.StartsWith("{\"type\":\"changed\",")) {
            var clickResponse = JsonConvert.DeserializeObject<Response.TileChangedResponse>(message);

            var changeCommands = clickResponse.data
                .Select(changeResponse => new Command.TileChange() {
                    x = changeResponse.x,
                    y = changeResponse.y,
                    color = changeResponse.i == -1 ? "#000000" : users[changeResponse.i].color,
                    defence = changeResponse.defence,
                    offence = changeResponse.offence,
                    productivity = changeResponse.productivity,
                    attackRange = changeResponse.attackRange,
                })
                .ToArray();

            UserInterface.instance.OnTileChanges(changeCommands);
        }
        else if (message.StartsWith("{\"type\":\"energy\",")) {
            var energyChangedResponse = JsonConvert.DeserializeObject<Response.EnergyChangedResponse>(message);

            UserInterface.instance.OnEnergyChange(energyChangedResponse.value);
        }
        else if (message.StartsWith("{\"type\":\"stage\",")) {
            var stage = JsonConvert.DeserializeObject<Response.Stage>(message);
            if (stage.stage == "wait") {
                UserInterface.instance.OnWait(stage.age);
            }
            else if (stage.stage == "running") {
                UserInterface.instance.OnRunning(stage.age);
                UserInterface.instance.OnEnergyChange(stage.energy);

                flameGroup.SetActive(true);
            }
        }
        else if (message.StartsWith("{\"type\":\"end\",")) {
            var endResponse = JsonConvert.DeserializeObject<Response.EndResponse>(message);

            UserInterface.instance.OnResultGameScore();

            var scores = endResponse.score.ToList();
            scores.Sort((a, b) => b.Value.power.CompareTo(a.Value.power));
            foreach (var kv in scores) {
                UserInterface.instance.OnResultAddEntry(users[kv.Key].color, kv.Value.tile.ToString());
            }

            webSocket.Close();

            sharedAudioSource.PlayOneShot(endClip);
        }
    }

    private void OnWebSocketOpen(WebSocket webSocket) {
        Debug.Log("WebSocket Open!");

        webSocket.Send(JsonConvert.SerializeObject(new Response.LoadRequest()));
    }

    public void ConquerCell(int x, int y) {
        SendOneTileClickRequest(Response.OneTileClickRequest.TypeNew, x, y);
    }

    public void UpgradeDefence(int x, int y) {
        SendOneTileClickRequest(Response.OneTileClickRequest.TypeDefenceUp, x, y);
    }

    public void UpgradeOffence(int x, int y) {
        SendOneTileClickRequest(Response.OneTileClickRequest.TypeOffenceUp, x, y);
    }

    public void UpgradeProductivity(int x, int y) {
        SendOneTileClickRequest(Response.OneTileClickRequest.TypeProductivityUp, x, y);
    }

    public void UpgradeAttackRange(int x, int y) {
        SendOneTileClickRequest(Response.OneTileClickRequest.TypeAttackRangeUp, x, y);
    }

    public void Attack(int fromX, int fromY, int toX, int toY) {
        SendTwoTileClickRequest(Response.TwoTileClickRequest.TypeAttack, fromX, fromY, toX, toY);
    }

    private void SendOneTileClickRequest(string type, int x, int y) {
        var oneTileClickRequest = new Response.OneTileClickRequest() {
            type = type,
            x = x,
            y = y,
        };
        var message = JsonConvert.SerializeObject(oneTileClickRequest);
        Debug.Log("SendOneTileClickRequest: " + message);
        webSocket.Send(message);
    }

    private void SendTwoTileClickRequest(string type, int fromX, int fromY, int toX, int toY) {
        var oneTileClickRequest = new Response.TwoTileClickRequest() {
            type = type,
            from = new Response.Pos() {x = fromX, y = fromY},
            to = new Response.Pos() {x = toX, y = toY},
        };
        var message = JsonConvert.SerializeObject(oneTileClickRequest);
        Debug.Log("SendTwoTileClickRequest: " + message);
        webSocket.Send(message);
    }

    // TODO Delete
    public void OnClientClick(int x, int y) {
    }

    // TODO Delete
    public void OnClientLevelUp(int x, int y) {
    }

    public void OnLoadButton() {
        webSocket.Send(JsonConvert.SerializeObject(new Response.LoadRequest()));
    }
}
