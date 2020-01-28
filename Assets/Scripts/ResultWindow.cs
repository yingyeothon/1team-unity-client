using UnityEngine;

public class ResultWindow : MonoBehaviour {
    [SerializeField] GameObject resultEntryPrefab = null;
    [SerializeField] Transform resultEntryParent = null;

    public void ClearAll() {
        while (resultEntryParent.childCount > 0) {
            var child = resultEntryParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    public void AddEntry(string color, string score) {
        var resultEntry = Instantiate(resultEntryPrefab, resultEntryParent).GetComponent<ResultEntry>();
        resultEntry.SetData($"#{resultEntryParent.childCount}", color, score);
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
