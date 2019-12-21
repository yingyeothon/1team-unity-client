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
    public struct EnterResponse {
        public string type;
        public User newbie;
    }
    
    [System.Serializable]
    public struct EndResponse {
        public string type;
        public GameScore score;
    }

    [System.Serializable]
    public class ClickRequest
    {
        public string type = "click";
        public ClickRequestData[] data;
    }

    [System.Serializable]
    public class ClickRequestData
    {
        public int value;
        public int x;
        public int y;
    }

    [System.Serializable]
    public struct ClickResponse
    {
        public string type;
        public TileChange[] changes;
    }

    [System.Serializable]
    public class LevelUpRequest
    {
        public string type = "levelUp";
        public LevelUpRequestData[] data;
    }

    [System.Serializable]
    public class LevelUpRequestData
    {
        public int value;
        public int x;
        public int y;
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
        public int l;
    }
    
    [System.Serializable]
    public struct Pos {
        public int x;
        public int y;
    }

    [System.Serializable]
    public struct TileChange
    {
        public int x;
        public int y;

        public int i;
        public double v;
        public int l;
    }

    [System.Serializable]
    public struct Stage
    {
        public string type;
        public string stage;
        public int age;
    }
}