using UnityEngine;
using UnityEngine.UI;

public class ResultEntry : MonoBehaviour {
    [SerializeField] Text rank = null;
    [SerializeField] Image image = null;
    [SerializeField] Text score = null;

    public void SetData(string rank, string color, string score) {
        this.rank.text = rank;
        ColorUtility.TryParseHtmlString(color, out var imageColor);
        image.color = imageColor;
        this.score.text = score;
    }
}
