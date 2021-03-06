﻿using System;
using UnityEngine;

[ExecuteInEditMode]
public class MainCamera : MonoBehaviour {
    public static MainCamera instance;
    Camera mainCamera;
    public Camera Cam => mainCamera;
    GameObject cubeGroup;
    public AudioSource bgmAudioSource;
    [SerializeField] float margin = 2.0f;
    private Vector3 lookAtTargetPos = Vector3.zero;

    void Awake() {
        InitReferences();
    }

    void InitReferences() {
        instance = this;
        mainCamera = GetComponent<Camera>();
        cubeGroup = GameObject.Find("Cube Group");
    }

    void Update() {
        //transform.LookAt(lookAtTargetPos);
        //transform.localPosition = lookAtTargetPos + new Vector3(transform.localPosition.x, transform.localPosition.y, -transform.localPosition.x);

        AdjustCamOrthographicSize();
    }

    // 레벨 전체가 항상 카메라에 들어올 수 있도록 한다.
    private void AdjustCamOrthographicSize() {
        // 스크린 포인트는 화면 왼쪽 아래가 (0, 0), 화면 오른쪽 위가 (Screen.width, Screen.height)다.
        if (!Application.isPlaying) {
            InitReferences();
        }

        var camLeftEnd = GetScreenPointToWorldGroundPoint(0, Screen.height / 2);
        var camRightEnd = GetScreenPointToWorldGroundPoint(Screen.width, Screen.height / 2);
        var camHorizontalDist = Vector3.Distance(camLeftEnd, camRightEnd);

        var camTopEnd = GetScreenPointToWorldGroundPoint(Screen.width / 2, Screen.height);
        var camBottomEnd = GetScreenPointToWorldGroundPoint(Screen.width / 2, 0);
        var camVerticalDist = Vector3.Distance(camTopEnd, camBottomEnd);

        var camDist = Mathf.Min(camHorizontalDist, camVerticalDist);

        var firstCube = cubeGroup.transform.GetChild(0);
        var lastCube = cubeGroup.transform.GetChild(cubeGroup.transform.childCount - 1);
        var cubeDist = margin + Vector3.Distance(firstCube.transform.position, lastCube.transform.position);

        if (camDist != 0 && cubeDist != 0) {
            Cam.orthographicSize *= cubeDist / camDist;
        }
    }

    internal void SetViewTargetCube(Cube cube) {
        transform.position = new Vector3(cube.transform.position.x, transform.position.y, cube.transform.position.z);
    }

    private Vector3 GetScreenPointToWorldGroundPoint(int screenPointX, int screenPointY) {
        var ray = Cam.ScreenPointToRay(new Vector3(screenPointX, screenPointY, 0));
        var plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out var distance)) {
            var cornerPoint = ray.GetPoint(distance);

            Debug.DrawLine(ray.origin, cornerPoint, Color.red);

            return cornerPoint;
        }

        return Vector3.zero;
    }

    public void ToggleBgm() {
        if (bgmAudioSource.isPlaying) {
            bgmAudioSource.Stop();
        } else {
            bgmAudioSource.Play();
        }
    }
}
