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

    void Awake() {
        instance = this;
    }

    void Start() {
        webSocket = new WebSocket(new System.Uri($"{MatchInfo.url}?x-game-id={MatchInfo.gameId}&x-member-id={MatchInfo.playerId}"));
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
            Debug.Log(JsonConvert.SerializeObject(loadResponse));

            foreach (var user in loadResponse.users) {
                users[user.index] = user;
            }

            UserInterface.instance.OnPlayerColorChange(loadResponse.me.color);
        }
        else if (message.StartsWith("{\"type\":\"enter\",")) {
            var enterResponse = JsonConvert.DeserializeObject<Response.EnterResponse>(message);
            Debug.Log("Enter: " + JsonConvert.SerializeObject(enterResponse));

            users[enterResponse.newbie.index] = enterResponse.newbie;
        }
        else if (message.StartsWith("{\"type\":\"click\",")) {
            var clickResponse = JsonConvert.DeserializeObject<Response.ClickResponse>(message);
            Debug.Log("ServerClick: " + JsonConvert.SerializeObject(clickResponse));

            var changeCommands = clickResponse.changes
                .Select(changeResponse => new Command.TileChange() {
                    x = changeResponse.x,
                    y = changeResponse.y,
                    color = changeResponse.i == -1 ? "#000000" : users[changeResponse.i].color,
                    v = (int) changeResponse.v,
                    l = changeResponse.l,
                    p = changeResponse.p,
                })
                .ToArray();

            UserInterface.instance.OnTileChanges(changeCommands);
        }
        else if (message.StartsWith("{\"type\":\"stage\",")) {
            var stage = JsonConvert.DeserializeObject<Response.Stage>(message);
            if (stage.stage == "wait") {
                UserInterface.instance.OnWait(stage.age);
            }
            else if (stage.stage == "running") {
                UserInterface.instance.OnRunning(stage.age);
            }
        }
        else if (message.StartsWith("{\"type\":\"end\",")) {
            var endResponse = JsonConvert.DeserializeObject<Response.EndResponse>(message);

            UserInterface.instance.OnResultGameScore();

            foreach (var kv in endResponse.score) {
                UserInterface.instance.OnResultAddEntry(users[kv.Key].color, kv.Value.power.ToString());
            }

            webSocket.Close();
        }
    }

    private void OnWebSocketOpen(WebSocket webSocket) {
        Debug.Log("WebSocket Open!");

        webSocket.Send(JsonConvert.SerializeObject(new Response.LoadRequest()));
    }

    public void OnClientClick(int x, int y) {
        var clickRequest = new Response.ClickRequest() {
            data = new List<Response.ClickRequestData>() {
                new Response.ClickRequestData {
                    value = 1,
                    x = x,
                    y = y,
                }
            }
        };
        webSocket.Send(JsonConvert.SerializeObject(clickRequest));
    }

    public void OnClientLevelUp(int x, int y) {
        var levelUpRequest = new Response.LevelUpRequest() {
            data = new List<Response.LevelUpRequestData>() {
                new Response.LevelUpRequestData() {
                    value = 1,
                    x = x,
                    y = y,
                }
            }
        };
        webSocket.Send(JsonConvert.SerializeObject(levelUpRequest));
    }

    public void OnLoadButton() {
        webSocket.Send(JsonConvert.SerializeObject(new Response.LoadRequest()));
    }
}
