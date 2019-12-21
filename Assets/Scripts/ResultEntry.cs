using UnityEngine;
using UnityEngine.UI;

public class ResultEntry : MonoBehaviour {
    [SerializeField] TMPro.TextMeshProUGUI rank = null;
    [SerializeField] Image image = null;
    [SerializeField] TMPro.TextMeshProUGUI score = null;

    public void SetData(string rank, string color, string score) {
        this.rank.text = rank;
        ColorUtility.TryParseHtmlString(color, out var imageColor);
        image.color = imageColor;
        this.score.text = score;
    }
}
