using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for altering the offset values for tracking points of rig and moves the body with head.
/// </summary>
[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform webGLVRTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

/// <summary>
/// This class is moving the body with the player,
/// </summary>
public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Transform headConstraint;
    public Vector3 headBodyOffset;

    public Transform editorHMDTarget;
    public Transform webGLHMDTarget;
    public Transform playerRoot;
    public Transform vrHMDTarget;
    public Transform leftVRHandTarget;
    public Transform rightVRHandTarget;

    // Start is called before the first frame update
    void Awake()
    {
        playerRoot = GameObject.FindGameObjectWithTag("Player").transform;
        editorHMDTarget = playerRoot.GetChild(2).GetChild(0);
        webGLHMDTarget = playerRoot.GetChild(2).GetChild(1);
        leftVRHandTarget = playerRoot.GetChild(0);
        rightVRHandTarget = playerRoot.GetChild(1);

        head.vrTarget = editorHMDTarget;
        head.webGLVRTarget = webGLHMDTarget;
        leftHand.vrTarget = leftVRHandTarget;
        rightHand.vrTarget = rightVRHandTarget;

#if !UNITY_EDITOR && UNITY_WEBGL
        head.vrTarget = head.webGLVRTarget;

#endif

        headBodyOffset = transform.position - headConstraint.position;


    }
   
    // Update is called once per frame
    void FixedUpdate()
    {        
        transform.position = headConstraint.position + headBodyOffset;
        float headToBodyAngle = Vector3.Dot(new Vector3(headConstraint.forward.x, transform.forward.y, headConstraint.forward.z), transform.forward) * Mathf.Rad2Deg;
        
        //Debug.Log(headToBodyAngle);
        if (headToBodyAngle < 0)
        {
            //transform.forward = Vector3.Lerp(transform.forward, new Vector3(headConstraint.forward.x, transform.forward.y, headConstraint.forward.z), Time.fixedDeltaTime);
            UpdateForward();
        }
        //Debug.Log("transform pos : " + transform.position + " VR Camera local pos : " + head.vrTarget.localPosition + " VR Camera pos : " + head.vrTarget.position + " Head constraint position : " + headConstraint.position + " Head pos : " + head.vrTarget.position );

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }

    /// <summary>
    /// Updates the body to face look direction
    /// </summary>
    public void UpdateForward()
    {
        Vector3 targetV = new Vector3(headConstraint.forward.x, transform.forward.y, headConstraint.forward.z);
        StartCoroutine(LerpPosition(targetV, 0.2f));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.forward;

        while (time < duration)
        {
            transform.forward = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        //transform.forward = targetPosition;
    }
}
