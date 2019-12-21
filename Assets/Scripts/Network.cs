using BestHTTP.WebSocket;
using Newtonsoft.Json;
using UnityEngine;

public class Network : MonoBehaviour {
    WebSocket webSocket;

    public static Network instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        webSocket = new WebSocket(new System.Uri("wss://cy18u3zcwk.execute-api.ap-northeast-2.amazonaws.com/dev"));
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

            UserInterface.instance.OnPlayerColorChange(loadResponse.me.color);
        }
    }

    private void OnWebSocketOpen(WebSocket webSocket) {
        Debug.Log("WebSocket Open!");
    }

    public void OnLoadButton() {
        webSocket.Send(JsonConvert.SerializeObject(new Response.LoadRequest()));


        UserInterface.instance.OnResultGameScore();
        UserInterface.instance.OnResultAddEntry("#ff0000", "100");
        UserInterface.instance.OnResultAddEntry("#00ff00", "50");
        UserInterface.instance.OnResultAddEntry("#0000ff", "10");
        UserInterface.instance.OnResultAddEntry("#ff0000", "100");
        UserInterface.instance.OnResultAddEntry("#00ff00", "50");
        UserInterface.instance.OnResultAddEntry("#0000ff", "10");
        UserInterface.instance.OnResultAddEntry("#ff0000", "100");
        UserInterface.instance.OnResultAddEntry("#00ff00", "50");
        UserInterface.instance.OnResultAddEntry("#0000ff", "10");
    }
}
