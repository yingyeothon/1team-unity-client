
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeSpawner : MonoBehaviour {
#if UNITY_EDITOR
    [SerializeField] GameObject cubePrefab = null;
    [SerializeField] Transform cubeGroup = null;
#endif
    // [SerializeField] GameObject tileInfoPrefab = null;
    // [SerializeField] Transform tileInfoGroup = null;


#if UNITY_EDITOR
    [ContextMenu("Spawn All Cubes")]
    void SpawnAllCubes() {
        while (cubeGroup.childCount != 0) {
            DestroyImmediate(cubeGroup.GetChild(0).gameObject);
        }

        var extent = 2;

        for (int i = -extent; i <= extent; i++) {
            for (int j = -extent; j <= extent; j++) {
                var cube = (UnityEditor.PrefabUtility.InstantiatePrefab(cubePrefab, cubeGroup) as GameObject).GetComponent<Cube>();
                cube.transform.localPosition = new Vector3(i, -0.4f, j);
                cube.X = i + extent;
                cube.Y = j + extent;
                // var cube = Instantiate(cubePrefab, new Vector3(i, -0.4f, j), Quaternion.identity, cubeGroup);
                cube.name = $"Cube [{i}, {j}]";

                // var tileInfo = (UnityEditor.PrefabUtility.InstantiatePrefab(tileInfoPrefab, tileInfoGroup) as GameObject).GetComponent<TileInfo>();
                // tileInfo.Tile = cube.transform;
                // tileInfo.InitBind();
                // tileInfo.UpdatePosition();
            }
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(cubeGroup.gameObject.scene);
    }
#endif
}
