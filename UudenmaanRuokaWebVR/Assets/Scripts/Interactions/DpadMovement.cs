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
    public Transform webGLhead;
    public Transform cameraPos;
    VRRig rig;

    public float moveSpeed = 1f; // 2f ok move speed
    public float stepTime = 0.3f;
    public float turnDegrees = 45f; // 50f turns ok speed

    public float turnRate = 0.1f;
    public bool blockTurn = false;
    public bool stepping = false;

    private void Start()
    {
        c = GetComponent<WebXRController>();
        body = transform.parent;
        rig = GameObject.FindGameObjectWithTag("IK_Body").GetComponent<VRRig>();

        if (head == null)
            Debug.LogError("Drag the moving camera component in to the head variable!");

#if !UNITY_EDITOR && UNITY_WEBGL
        cameraPos = webGLhead;
#elif UNITY_EDITOR
        cameraPos = head;
#endif
    }

    private void Update()
    {
#if UNITY_EDITOR
        c.TryUpdateButtons();
#endif

        if (c.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).x > 0.9 && !blockTurn)
        {
            Turn(turnDegrees);
        }
        if (c.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).x < -0.9 && !blockTurn)
        {
            Turn(-turnDegrees);
        }

#if !UNITY_EDITOR && UNITY_WEBGL
        if (c.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).y < -0.9)
        {
            Move(new Vector3(cameraPos.forward.x, 0, cameraPos.forward.z).normalized);     
        }
        if (c.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).y > 0.9)
        {
            Move(new Vector3(-cameraPos.forward.x, 0, -cameraPos.forward.z).normalized);          
        }

#endif
#if UNITY_EDITOR
        if (c.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).y > 0.9)
        {  
            Move(new Vector3(cameraPos.forward.x, 0, cameraPos.forward.z).normalized);
        }
        if (c.GetAxis2D(WebXRController.Axis2DTypes.Thumbstick).y < -0.9)
        {
            Move(new Vector3(-cameraPos.forward.x, 0, -cameraPos.forward.z).normalized);
        }
#endif
      
    }

    /// <summary>
    /// Moves to look direction, incrementing body position by vector with flattened Y axis
    /// </summary>
    public void Move(Vector3 toPos)
    {
        rig.UpdateForward();
        body.position += toPos * moveSpeed * Time.deltaTime;      
    }

    /// <summary>
    /// Turning
    /// </summary>
    /// <param name="turnAmount"></param>
    public void Turn(float turnAmount)
    {
        blockTurn = true;
        body.RotateAround(cameraPos.position, Vector3.up, turnAmount);
        rig.UpdateForward();
        StartCoroutine(SnapTurnCooldown());
    }

    public IEnumerator SnapTurnCooldown()
    {
        yield return new WaitForSeconds(turnRate);
        blockTurn = false;
    }

}
