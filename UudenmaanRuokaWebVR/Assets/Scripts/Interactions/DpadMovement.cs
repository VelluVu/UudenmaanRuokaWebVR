using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Simple Dpad movement for WebXR with mapped Dpad directions.
/// </summary>
public class DpadMovement : MonoBehaviour
{

    public WebXRController c;
    Transform body;
    [Tooltip("Drag the moving head camera here.")]
    public Transform head; // Dont forget to drag in the head!

    public float moveSpeed = 0.3f; // 2f ok move speed
    public float stepTime = 0.3f;
    public float turnDegrees = 45f; // 50f turns ok speed

    public float turnRate = 0.1f;
    public bool blockTurn = false;
    public bool stepping = false;

    private void Start()
    {
        c = GetComponent<WebXRController>();
        body = transform.parent;
        if (head == null)
            Debug.LogError("Drag the moving camera component in to the head variable!");
    }

    private void Update()
    {
        c.TryUpdateButtons();

        if (c.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).x > 0.75)
        {
            //Debug.Log("Pressing DpadR");
            if (!blockTurn)
                StartCoroutine(SmoothlyTurnToRotation(turnDegrees));
        }
        if (c.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).x < -0.75)
        {
            //Debug.Log("Pressing DpadL");
            if (!blockTurn)
                StartCoroutine(SmoothlyTurnToRotation(-turnDegrees));
        }

        //StartCoroutine(TurnDelay());


        if (c.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).y > 0.75)
        {
            //Debug.Log("DpadU detected " + c.GetButton("DpadU"));
            if (!stepping)
                StartCoroutine(Stepping(new Vector3(head.forward.x, 0, head.forward.z)));
        }
        if (c.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).y < -0.75)
        {
            //Debug.Log("DpadD detected " + c.GetButton("DpadD"));
            if (!stepping)
                StartCoroutine(Stepping(new Vector3(-head.forward.x, 0, -head.forward.z)));
        }


    }
    IEnumerator TurnDelay()
    {
        yield return new WaitForSeconds(turnRate);
    }

    /// <summary>Slerp Turns player by degrees</summary>
    IEnumerator SmoothlyTurnToRotation(float turnAmound)
    {
        //Debug.Log("Started Turning " + turnAmound);
        blockTurn = true;
        float elapsedTime = 0;
        Quaternion fromAngle = body.rotation;
        Quaternion toAngle = body.rotation * Quaternion.AngleAxis(turnAmound, Vector3.up);
        while (elapsedTime < turnRate)
        {
            Debug.Log("Turning in " + turnRate + " elapsedTime " + elapsedTime);
            body.rotation = Quaternion.Slerp(fromAngle, toAngle, elapsedTime / turnRate);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        Debug.Log("Enabling turning again");
        blockTurn = false;
    }

    /// <summary>
    /// Moves to look direction, incrementing body position by vector with flattened Y axis
    /// </summary>
    public void MoveForward()
    {
        Vector3 fromPos = body.position;
        Vector3 toPos = new Vector3(head.forward.x, 0, head.forward.z);
        body.position = Vector3.Lerp(fromPos, toPos, Time.deltaTime * moveSpeed);
    }

    /// <summary>
    /// Lerps player to head look direction with steps
    /// </summary>
    /// <param name="toPos"></param>
    /// <returns></returns>
    IEnumerator Stepping(Vector3 toPos)
    {
        stepping = true;
        float t = 0;
        Vector3 fromPos = body.position;
        toPos = toPos.normalized;
        toPos = Vector3.Scale(toPos, Vector3.one * moveSpeed);
        toPos += fromPos;

        while (t < stepTime)
        {
            body.position = Vector3.Lerp(fromPos, toPos, t / stepTime);
            t += Time.deltaTime;
            yield return null;
        }
        stepping = false;
    }

    /// <summary>
    /// Moves backwards, reducing body position by vector with flattened Y axis
    /// </summary>
    public void MoveBackward()
    {
        Vector3 fromPos = body.position;
        Vector3 toPos = new Vector3(-head.forward.x, 0, -head.forward.z).normalized;
        body.position = Vector3.Lerp(fromPos, toPos, Time.deltaTime * moveSpeed);
    }

    /// <summary>
    /// Turns Right by altering body rotation
    /// </summary>
    public void TurnRight()
    {
        //body.rotation = body.rotation * Quaternion.Euler(0,1 * turnDegrees * Time.deltaTime, 0);
        body.rotation = Quaternion.Lerp(body.rotation, Quaternion.AngleAxis(turnDegrees, Vector3.up), Time.deltaTime);
    }

    /// <summary>
    /// Turns Left by altering body rotation
    /// </summary>
    public void TurnLeft()
    {

        //body.rotation = body.rotation * Quaternion.Euler(0, -1 * turnDegrees * Time.deltaTime, 0);
        body.rotation = Quaternion.Lerp(body.rotation, Quaternion.AngleAxis(-turnDegrees, Vector3.up), Time.deltaTime);
    }

}
