using UnityEngine;

public class ResultWindow : MonoBehaviour {
    [SerializeField] GameObject resultEntryPrefab = null;
    [SerializeField] Transform resultEntryParent = null;

    public void ClearAll() {
        var childCount = resultEntryParent.childCount;
        for (int i = 0; i < childCount; i++) {
            Destroy(resultEntryParent.GetChild(i).gameObject);
        }
    }

    public void AddEntry(string color, string score) {
        var resultEntry = Instantiate(resultEntryPrefab, resultEntryParent).GetComponent<ResultEntry>();
        resultEntry.SetData($"#{resultEntryParent.childCount + 1}", color, score);
    }

    public void OnCloseButton() {
        if (UserInterface.instance.onResultWindowClose != null) {
            UserInterface.instance.onResultWindowClose();
        }
    }

    public void OnRestartButton() {
        if (UserInterface.instance.onResultWindowRestart != null) {
            UserInterface.instance.onResultWindowRestart();
        }
    }
}
