using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PositionToZero : MonoBehaviour
{
    public Transform editorCam;
    public Transform webGLCam;

    public Transform root;
    public Transform cam;
    public Transform body;

    private void Awake()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        cam = webGLCam;
#elif UNITY_EDITOR
        cam = editorCam;
#endif

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
#if UNITY_EDITOR
        Debug.Log("Sceneloaded " + arg0.name);
        if (arg0.name == "LobbyScene" || arg0.name == "MainScene")
        {
            Debug.Log("Lobby loaded setting player position...");
            StartCoroutine(Position());
        }
#endif
    }

    private IEnumerator Position()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 offset = cam.position;
        offset = new Vector3(offset.x, 0, offset.z);
        root.position -= offset;
        body.position = root.position;

    }
}
