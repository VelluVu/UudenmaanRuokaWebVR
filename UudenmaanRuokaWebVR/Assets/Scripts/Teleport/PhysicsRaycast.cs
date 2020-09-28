using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

public class PhysicsRaycast : MonoBehaviour
{

    WebXRController controller;
    public bool active = false;
    public LineRenderer lineRenderer;
    public LayerMask hitLayer;
    RaycastHit hit;
    public float maxDistance = 5f;
    string trigger = "TriggerLeft";

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        controller = GetComponent<WebXRController>();

        if (controller.hand == WebXRControllerHand.LEFT)
            trigger = "TriggerLeft";
        if (controller.hand == WebXRControllerHand.RIGHT)
            trigger = "TriggerRight";
    }

    private void Update()
    {
        if (active)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, hitLayer))
            {
                Debug.Log("Hits the Teleport Layer");
                if (hit.transform)
                {
                    lineRenderer.positionCount = 2;

                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.point);

                    if (Input.GetButtonDown(trigger))
                    {
                        Debug.Log("Pressed the " + trigger);
                        transform.parent.position = hit.point;
                    }
                }
            }
            else
            {
                lineRenderer.positionCount = 0;
            }
        }
    }
}
