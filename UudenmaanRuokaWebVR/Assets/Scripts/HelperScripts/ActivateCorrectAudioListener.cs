using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Adds the AudioListener for correct gameobject depending on platform.
/// </summary>
public class ActivateCorrectAudioListener : MonoBehaviour
{
    public Transform inEditor;
    public Transform inWebGL;
 
    void Awake()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        if (inWebGL.GetComponent<AudioListener>() == null)
        {
            inWebGL.gameObject.AddComponent<AudioListener>();
        }
       
#elif UNITY_EDITOR
        if (inEditor.GetComponent<AudioListener>() == null)
        {
            inEditor.gameObject.AddComponent<AudioListener>();
        }
     
#endif
    }


}
