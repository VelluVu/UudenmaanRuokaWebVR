﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// This class shoots the physics raycast into chosen layers and does the layer matching operation
/// 
/// </summary>
public class PhysicsRaycaster : MonoBehaviour
{
    public bool debugging = true;
    public bool active = true;

    WebXRController controller;
    DesertControllerInteraction pickUpInteraction;
    public RaycastToUI uiPointer;
    RaycastHit hit;

    public LineRenderer lineRenderer;
    public LayerMask hitLayer;
    private LayerMask teleMask;
    private LayerMask uiMask;

    public float maxDistance = 10f;

    /// <summary>
    /// Gets the necessary components and hides the linerenderer.
    /// </summary>
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        controller = GetComponent<WebXRController>();
        pickUpInteraction = GetComponent<DesertControllerInteraction>();
        lineRenderer.positionCount = 0;
        teleMask = LayerMask.NameToLayer("Teleport");
        uiMask = LayerMask.NameToLayer("UI");
        if(transform.childCount > 0)
        {
            uiPointer = transform.GetChild(0).GetComponent<RaycastToUI>();
        }
    }

    /// <summary>
    /// Raycasts and drawsray if hits correct layers.
    /// </summary>
    private void Update()
    {
        if (active)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, hitLayer))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    HideRay();
                    return;
                }

                DrawRay(transform.position, hit.point);

                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    VRInputModule.pointingUI = true;
                }
                else
                {
                    VRInputModule.pointingUI = false;
                }
                

                //Distant picks up interactable if pointing interactable
                if (hit.collider.gameObject.CompareTag("Interactable"))
                {
                    if (controller.GetButtonDown("Trigger") || controller.GetButtonDown("Grip"))
                    {
                        pickUpInteraction.DistantPickUp(hit.point, hit.collider);
                    }
                }

                if (hit.collider.gameObject.layer == teleMask) // Mozilla WebXR exporter for some reason reguires both down and up checks.
                {
                    if (controller.GetButtonDown("Trigger") || controller.GetButtonDown("Grip"))
                    {

                    }
                    if (controller.GetButtonUp("Trigger") || controller.GetButtonUp("Grip"))
                    {
                        Teleport(hit.point);
                    }
                }

                //if (uiPointer != null)
                //{
                //    if (hit.collider.gameObject.layer == uiMask && uiPointer.IsActive())
                //    {
                //        uiPointer.ActivatePointer();
                //    }
                //    else if(hit.collider.gameObject.layer != uiMask && uiPointer.IsActive())
                //    {
                //        uiPointer.DeActivatePointer();
                //    }
                //}

                
            }

            else
            {
                HideRay();
            }
        }
        else
        {
            HideRay();
        }

        //Drops if holding interactable and releasing trigger/grip
        if ((controller.GetButtonUp("Trigger") || controller.GetButtonUp("Grip")) && pickUpInteraction.HoldingObj())
        {
            pickUpInteraction.Drop();
        }
    }

    /// <summary>
    /// Teleports to the position
    /// </summary>
    /// <param name="pos">target position</param>
    void Teleport(Vector3 pos)
    {
        Debug.Log("Teleport to " + pos);
        transform.parent.position = pos;
    }

    /// <summary>
    /// Draws the line representing the raycast to help aiming
    /// </summary>
    /// <param name="from">start position</param>
    /// <param name="to">end position</param>
    void DrawRay(Vector3 from, Vector3 to)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
    }

    /// <summary>
    /// Removes the line
    /// </summary>
    void HideRay()
    {
        lineRenderer.positionCount = 0;
    }
}
