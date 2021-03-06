using System.Collections.Generic;

namespace Response {

    using GameScore = Dictionary<int, GameScoreValue>;

    [System.Serializable]
    public class MatchInfo {
        public string type;
        public string url;
        public string gameId;
        public string playerId;
    }

    [System.Serializable]
    public class LoadRequest {
        public string type = "load";
    }

    [System.Serializable]
    public struct LoadResponse {
        public string type;
        public User me;
        public List<User> users;
        public List<List<Tile>> board;
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
    public class OneTileClickRequest {
        public static string TypeNew = "new";
        public static string TypeDefenceUp = "defenceUp";
        public static string TypeOffenceUp = "offenceUp";
        public static string TypeProductivityUp = "productivityUp";
        public static string TypeAttackRangeUp = "attackRangeUp";

        public string type;
        public int x;
        public int y;
    }

    [System.Serializable]
    public class TwoTileClickRequest {
        public static string TypeAttack = "attack";

        public string type;
        public Pos from;
        public Pos to;
    }

    [System.Serializable]
    public struct TileChangedResponse {
        public string type;
        public List<TileChange> data;
    }

    [System.Serializable]
    public struct EnergyChangedResponse {
        public string type;
        public int value;
    }

    [System.Serializable]
    public class LevelUpRequest {
        public string type = "levelUp";
        public List<LevelUpRequestData> data;
    }

    [System.Serializable]
    public class LevelUpRequestData {
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
        public int defence;
        public int offence;
        public int productivity;
        public int attackRange;
    }

    [System.Serializable]
    public struct Pos {
        public int x;
        public int y;
    }

    [System.Serializable]
    public struct TileChange {
        public int x;
        public int y;

        public int i;

        public int defence;
        public int offence;
        public int productivity;
        public int attackRange;
    }

    [System.Serializable]
    public struct Stage {
        public string type;
        public string stage;
        public int age;
        public int energy;
    }

}
