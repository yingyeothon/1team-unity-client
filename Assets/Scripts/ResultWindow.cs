using UnityEngine;

public class ResultWindow : MonoBehaviour {
    [SerializeField] GameObject resultEntryPrefab = null;
    [SerializeField] Transform resultEntryParent = null;

    public void ClearAll() {
        while (resultEntryParent.childCount > 0) {
            DestroyImmediate(resultEntryParent.GetChild(0));
        }
    }

    public void AddEntry(string color, string score) {
        var resultEntry = Instantiate(resultEntryPrefab, resultEntryParent).GetComponent<ResultEntry>();
        resultEntry.SetData($"#{resultEntryParent.childCount + 1}", color, score);
    }

    public void OnCloseButton() {
        UserInterface.instance.onResultWindowClose();
    }
}
