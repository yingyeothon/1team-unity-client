
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeSpawner : MonoBehaviour {
    [SerializeField] GameObject cubePrefab = null;
    [SerializeField] Transform cubeGroup = null;

#if UNITY_EDITOR
    [ContextMenu("Spawn All Cubes")]
    void SpawnAllCubes() {
        while (cubeGroup.childCount != 0) {
            DestroyImmediate(cubeGroup.GetChild(0).gameObject);
        }

        for (int i = -5; i <= 5; i++) {
            for (int j = -5; j <= 5; j++) {
                var cube = UnityEditor.PrefabUtility.InstantiatePrefab(cubePrefab, cubeGroup) as GameObject;
                cube.transform.localPosition = new Vector3(i, -0.4f, j);
                // var cube = Instantiate(cubePrefab, new Vector3(i, -0.4f, j), Quaternion.identity, cubeGroup);
                cube.name = $"Cube [{i}, {j}]";
            }
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(cubeGroup.gameObject.scene);
    }
#endif
}
