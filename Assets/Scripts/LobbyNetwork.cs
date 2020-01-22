using System.Collections;
using System.Text;
using BestHTTP;
using BestHTTP.WebSocket;
using Newtonsoft.Json;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour {
    private static string GAME_ID = "abc";

    [SerializeField] Network network = null;

    void Start() {
        UserInterface.instance.OnLoggingIn();

        RequestAuth();
    }

    private IEnumerator WaitAndRestart() {
        yield return new WaitForSeconds(1);

        Start();
    }

    private void RequestAuth() {
        var request = new HTTPRequest(new System.Uri("https://api.yyt.life/auth/simple"), HTTPMethods.Post, OnAuthRequestFinished);
        request.SetHeader("Content-Type", "application/json; charset=UTF-8");
        request.RawData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
            new AuthRequest {
                name = "USER_NAME",
                applications = new[] {GAME_ID},
            }
        ));
        request.Send();
    }

    private void OnAuthRequestFinished(HTTPRequest request, HTTPResponse response) {
        Debug.Log("Auth Request Finished");

        var authToken = response.DataAsText;
        RequestMatch(authToken);
    }

    private void RequestMatch(string authToken) {
        var webSocket = new WebSocket(new System.Uri("wss://ws.yyt.life/lobby"));
        webSocket.InternalRequest.SetHeader("Authorization", $"Bearer {authToken}");

        webSocket.OnOpen += delegate(WebSocket socket) {
            Debug.Log("Lobby Socket Open");

            UserInterface.instance.OnMatching();

            socket.Send(JsonConvert.SerializeObject(
                new MatchRequest {
                    application = GAME_ID
                }
            ));
        };

        webSocket.OnMessage += delegate(WebSocket socket, string message) {
            socket.Close();

            var matchResponse = JsonConvert.DeserializeObject<Response.MatchInfo>(message);
            OnMatchFinished(matchResponse);
        };

        webSocket.OnError += delegate(WebSocket socket, string error) {
            Debug.Log("Error: " + error);

            StartCoroutine(WaitAndRestart());
        };

        webSocket.Open();
    }

    private void OnMatchFinished(Response.MatchInfo matchInfo) {
        Debug.Log("Match Finished: " + JsonConvert.SerializeObject(matchInfo));

        ConnectGame(matchInfo);
    }

    private void ConnectGame(Response.MatchInfo matchInfo) {
        network.MatchInfo = matchInfo;
        network.gameObject.SetActive(true);

        this.gameObject.SetActive(false);
    }

    [System.Serializable]
    public class AuthRequest {
        public string name;
        public string[] applications;
    }

    [System.Serializable]
    public class MatchRequest {
        public string type = "match";
        public string application;
    }
}
