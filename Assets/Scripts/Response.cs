using System.Collections.Generic;

namespace Response {

    using GameScore = Dictionary<int, GameScoreValue>;

    [System.Serializable]
    public class LoadRequest {
        public string type = "load";
    }

    [System.Serializable]
    public struct LoadResponse {
        public string type;
        public User me;
        public User[] users;
        public Tile[][] board;
        public string stage;
        public int age;
    }

    [System.Serializable]
    public struct User {
        public int index;
        public string color;
    }

    [System.Serializable]
    public struct GameScoreValue {
        public int tile;
        public int power;
    }

    [System.Serializable]
    public struct Tile {
        public int i;
        public int v;
    }
    
    [System.Serializable]
    public struct Pos {
        public int x;
        public int y;
    }
}