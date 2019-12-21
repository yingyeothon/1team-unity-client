using System.Collections.Generic;
using System.Linq;
using BestHTTP.WebSocket;
using Newtonsoft.Json;
using UnityEngine;

public class Network : MonoBehaviour {
    WebSocket webSocket;

    private Dictionary<int, Response.User> users = new Dictionary<int, Response.User>();

    public static Network instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        webSocket = new WebSocket(new System.Uri("wss://8tl0tknp30.execute-api.ap-northeast-2.amazonaws.com/dev"));
        webSocket.OnOpen += OnWebSocketOpen;
        webSocket.OnMessage += OnMessageReceived;
        webSocket.OnBinary += OnBinaryMessageReceived;
        webSocket.OnClosed += OnWebSocketClosed;
        webSocket.OnError += OnError;
        webSocket.Open();
    }

    void OnError(WebSocket ws, string error) {
        Debug.Log("Error: " + error);
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
        } else if (message.StartsWith("{\"type\":\"enter\",")) {
            var enterResponse = JsonConvert.DeserializeObject<Response.EnterResponse>(message);
            Debug.Log("Enter: " + JsonConvert.SerializeObject(enterResponse));

            users[enterResponse.newbie.index] = enterResponse.newbie;
        } else if (message.StartsWith("{\"type\":\"click\",")) {
            var clickResponse = JsonConvert.DeserializeObject<Response.ClickResponse>(message);
            Debug.Log("ServerClick: " + JsonConvert.SerializeObject(clickResponse));
            
            var changeCommands = clickResponse.changes
                .Select(changeResponse => new Command.TileChange()
                {
                    x = changeResponse.x,
                    y = changeResponse.y,
                    color = changeResponse.i == -1 ? "#ffffff" : users[changeResponse.i].color,
                    v = (int) changeResponse.v,
                    l = changeResponse.l,
                })
                .ToArray();

            UserInterface.instance.OnTileChanges(changeCommands);
        } else if (message.StartsWith("{\"type\":\"stage\",")) {
            var stage = JsonConvert.DeserializeObject<Response.Stage>(message);
            if (stage.stage == "wait") {
                UserInterface.instance.OnWait(stage.age);
            } else if (stage.stage == "running") {
                UserInterface.instance.OnRunning(stage.age);
            }
        }
    }

    private void OnWebSocketOpen(WebSocket webSocket) {
        Debug.Log("WebSocket Open!");
        
        webSocket.Send(JsonConvert.SerializeObject(new Response.LoadRequest()));
    }

    public void OnClientClick(int x, int y)
    {
        var clickRequest = new Response.ClickRequest()
        {
            data = new[]
            {
                new Response.ClickRequestData
                {
                    value = 1,
                    x = x,
                    y = y,
                }
            }
        };
        webSocket.Send(JsonConvert.SerializeObject(clickRequest));
    }

    public void OnClientLevelUp(int x, int y)
    {
        var levelUpRequest = new Response.LevelUpRequest()
        {
            data = new[]
            {
                new Response.LevelUpRequestData()
                {
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


        // UserInterface.instance.OnResultGameScore();
        // UserInterface.instance.OnResultAddEntry("#ff0000", "100");
        // UserInterface.instance.OnResultAddEntry("#00ff00", "50");
        // UserInterface.instance.OnResultAddEntry("#0000ff", "10");
        // UserInterface.instance.OnResultAddEntry("#ff0000", "100");
        // UserInterface.instance.OnResultAddEntry("#00ff00", "50");
        // UserInterface.instance.OnResultAddEntry("#0000ff", "10");
        // UserInterface.instance.OnResultAddEntry("#ff0000", "100");
        // UserInterface.instance.OnResultAddEntry("#00ff00", "50");
        // UserInterface.instance.OnResultAddEntry("#0000ff", "10");
    }
}
