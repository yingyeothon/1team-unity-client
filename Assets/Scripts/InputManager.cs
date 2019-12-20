using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {
//     [SerializeField] Camera mainCam = null;

//     Transform currentHit;

//     void Start() {
//         UpdateCubeMouseEvent();
//     }

//     void Update() {
//         if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {
//             UpdateCubeMouseEvent();
//         }
//     }

//     void UpdateCubeMouseEvent() {
//         if (EventSystem.current.IsPointerOverGameObject()) {
//             Debug.Log("s");
//             return;
//         }
// Debug.Log("x");
//         var newHit = QueryCurrentHit();
//         if (currentHit != newHit) {
//             if (currentHit != null) {
//                 currentHit.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
//             }
//             newHit.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
//             currentHit = newHit;
//         }
//     }

//     Transform QueryCurrentHit() {
//         RaycastHit hit;
//         Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

//         if (Physics.Raycast(ray, out hit)) {
//             return hit.transform;
//         }

//         return null;
//     }
}
