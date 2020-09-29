using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using WebXR;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Simple Dpad movement for WebXR with mapped Dpad directions.
/// </summary>
public class DpadMovement : MonoBehaviour
{
    WebXRController c;
    Transform body;
    [Tooltip("Drag the moving head camera here.")]
    public Transform head; // Dont forget to drag in the head!

    public float moveSpeed = 2f; // 2f ok move speed
    public float turnSpeed = 50f; // 50f turns ok speed

    private void Start()
    {
        c = GetComponent<WebXRController>();
        body = transform.parent;
        if (head == null)
            Debug.LogError("Drag the moving camera component in to the head variable!");
    }

    private void Update()
    {
        if (c.GetButton("DpadR"))
        {
            //Debug.Log("DpadR detected " + c.GetButton("DpadR"));
            TurnRight();
        }
        if (c.GetButton("DpadL"))
        {
            //Debug.Log("DpadL detected " + c.GetButton("DpadL"));
            TurnLeft();
        }
        if (c.GetButton("DpadU"))
        {
            //Debug.Log("DpadU detected " + c.GetButton("DpadU"));
            MoveForward();
        }
        if (c.GetButton("DpadD"))
        {
            //Debug.Log("DpadD detected " + c.GetButton("DpadD"));
            MoveBackward();
        }
    }

    /// <summary>
    /// Moves to look direction, incrementing body position by vector with flattened Y axis
    /// </summary>
    public void MoveForward()
    {
        body.position += new Vector3(head.forward.x,0,head.forward.z) * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Moves backwards, reducing body position by vector with flattened Y axis
    /// </summary>
    public void MoveBackward()
    {
        body.position -= new Vector3(head.forward.x, 0, head.forward.z) * moveSpeed * Time.deltaTime; //ei liiku y akselilla ei ole lentokonepeli!
    }

    /// <summary>
    /// Turns Right by altering body rotation
    /// </summary>
    public void TurnRight()
    {
        body.rotation = body.rotation * Quaternion.Euler(0,1 * turnSpeed * Time.deltaTime, 0);
    }

    /// <summary>
    /// Turns Left by altering body rotation
    /// </summary>
    public void TurnLeft()
    {
        body.rotation = body.rotation * Quaternion.Euler(0, -1 * turnSpeed * Time.deltaTime, 0);
    }

}
