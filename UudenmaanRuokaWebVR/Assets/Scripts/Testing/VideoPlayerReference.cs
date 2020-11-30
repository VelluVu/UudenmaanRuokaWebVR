using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerReference : MonoBehaviour
{
    VideoPlayer vp;

    private void Start()
    {
        vp = GetComponent<VideoPlayer>();
        Debug.Log(Application.streamingAssetsPath);
        vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "URVideo.mp4");
        vp.Play();
    }
}
