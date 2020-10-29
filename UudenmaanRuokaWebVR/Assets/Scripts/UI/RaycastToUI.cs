using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Raycasting to UI for use of Unity UI events.
/// </summary>
public class RaycastToUI : MonoBehaviour
{
    WebXRController controller;
    public VRInputModule module;
    public Canvas[] canvases;
    public Camera m_cam;

    private void Start()
    {     
        controller = transform.parent.GetComponent<WebXRController>();
        module = FindObjectOfType<VRInputModule>();
        canvases = FindObjectsOfType<Canvas>();
        
        if (m_cam == null)
            m_cam = GetComponent<Camera>();

        if (canvases.Length > 0)
        {
            for (int i = 0; i < canvases.Length; i++)
            {
                if (canvases[i] != null)
                    canvases[i].worldCamera = m_cam;
            }
        }
    }

    //public bool IsActive()
    //{
    //    return active;
    //}

    //public void ActivatePointer()
    //{
    //    for (int i = 0; i < canvases.Length; i++)
    //    {
    //        if (canvases[i] != null)
    //            canvases[i].worldCamera = m_cam;
    //    }
    //    active = true;
    //}

    //public void DeActivatePointer()
    //{
    //    for (int i = 0; i < canvases.Length; i++)
    //    {
    //        if (canvases[i] != null)
    //            canvases[i].worldCamera = null;
    //    }
    //    active = false;
    //}
    //TODO : 
    // 1. When pointing UI layer Graphical || Physical raycast a rendered line.
    // 2. When pressing trigger while aiming UI element UI needs to react.
    // 3. Make to work with more UI elements , like scrollview, slider etc...

    private void Update()
    {
        
    }

    
}
