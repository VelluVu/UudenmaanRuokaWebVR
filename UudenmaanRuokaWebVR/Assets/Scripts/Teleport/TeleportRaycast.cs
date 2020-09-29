using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// This class shoots the teleport raycast into Teleport Layer and activates the teleportation when trigger is pressed.
/// If moved away from Teleport Layer teleport device will be deactivated.
/// </summary>
public class TeleportRaycast : MonoBehaviour
{
    public bool debugging = false;
    WebXRController controller;
    public bool active = false;
    public LineRenderer lineRenderer;
    public LayerMask hitLayer;
    RaycastHit hit;
    public float maxDistance = 10f;
 
    /// <summary>
    /// Gets the necessary components and hides the linerenderer.
    /// </summary>
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        controller = GetComponent<WebXRController>();
        lineRenderer.positionCount = 0;
    }

    /// <summary>
    /// When active enables teleporting into pointed position, after trigger press teleports into position and deactivates device.
    /// When not hitting teleport area deactivates the device.
    /// </summary>
    private void Update()
    {  
        if (active)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, hitLayer))
            {
                if(debugging)
                    Debug.Log("Hits the Teleport Layer");
                if (hit.transform)
                {
                    lineRenderer.positionCount = 2;

                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.point);

                    if (controller.GetButtonDown("Trigger"))
                    {
                        if(debugging)
                            Debug.Log("Pressed the " + controller.name + " Trigger.");
                        transform.parent.position = hit.point;
                        active = !active;
                    }
                }
            }
            else
            {
                lineRenderer.positionCount = 0;
                active = !active;
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
}
