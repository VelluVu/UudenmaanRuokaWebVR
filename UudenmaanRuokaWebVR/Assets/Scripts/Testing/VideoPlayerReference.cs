using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Sets up reference URL for video player, so it works in WebGL.
/// </summary>
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
