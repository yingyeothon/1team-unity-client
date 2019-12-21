using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
    [SerializeField] Image playerImage = null;

    public static UserInterface instance;

    public void Awake() {
        instance = this;
    }
    
    public void OnPlayerColorChange(string playerColor) {
        ColorUtility.TryParseHtmlString(playerColor, out var color);
        playerImage.color = color;
    }
}
