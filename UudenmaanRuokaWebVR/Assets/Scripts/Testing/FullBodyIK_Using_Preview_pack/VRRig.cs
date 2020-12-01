using System.Collections;
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
/// @Author : Veli-Matti Vuoti
/// 
/// This class is moving the IK-body with the player,
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
    public Transform lShoulder;
    public Transform rShoulder;

    // Start is called before the first frame update
    void Awake()
    {
       
#if UNITY_EDITOR
        playerRoot = GameObject.FindGameObjectWithTag("Player").transform;
        editorHMDTarget = playerRoot.GetChild(2).GetChild(0);
        webGLHMDTarget = playerRoot.GetChild(2).GetChild(1);
        leftVRHandTarget = playerRoot.GetChild(0);
        rightVRHandTarget = playerRoot.GetChild(1);
        head.vrTarget = editorHMDTarget;
#elif !UNITY_EDITOR && UNITY_WEBGL
        head.vrTarget = head.webGLVRTarget;
#endif
        head.webGLVRTarget = webGLHMDTarget;
        leftHand.vrTarget = leftVRHandTarget;
        rightHand.vrTarget = rightVRHandTarget;

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

        CheckShoulderDistanceToController();
        //Debug.Log("transform pos : " + transform.position + " VR Camera local pos : " + head.vrTarget.localPosition + " VR Camera pos : " + head.vrTarget.position + " Head constraint position : " + headConstraint.position + " Head pos : " + head.vrTarget.position );

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }

    /// <summary>
    /// Check if player is trying to reach too far with hand and then body needs to turn to reach the point.
    /// </summary>
    public void CheckShoulderDistanceToController()
    {

        float lElbowDistance = Vector3.Distance(lShoulder.position, leftHand.vrTarget.position);
        float rElbowDistance = Vector3.Distance(rShoulder.position, rightHand.vrTarget.position);

        if (lElbowDistance > 0.70f)
        {
            float dotProductl = Vector3.Dot(leftHand.vrTarget.position, transform.up);
            //Debug.Log(dotProductl);
            if (dotProductl > 1.2f)
            {            
                //Debug.Log("Left hand reaching too far");
                ReachToTarget(leftHand.vrTarget);
            }
        }
        if (rElbowDistance > 0.70f)
        {
            float dotProductr = Vector3.Dot(rightHand.vrTarget.position, transform.up);
            //Debug.Log(dotProductr);
            if (dotProductr > 1.2f)
            {
                //Debug.Log("Right hand reaching too far");
                ReachToTarget(rightHand.vrTarget);
            }
        }
    }

    public void ReachToTarget(Transform target)
    {
        Vector3 targetV = new Vector3(target.forward.x, transform.forward.y, target.forward.z);
        StartCoroutine(LerpPosition(targetV, 0.2f));
    }

    /// <summary>
    /// Updates the body to face look direction
    /// </summary>
    public void UpdateForward()
    {
        Vector3 targetV = new Vector3(headConstraint.forward.x, transform.forward.y, headConstraint.forward.z);
        StartCoroutine(LerpPosition(targetV, 0.2f));
    }

    /// <summary>
    /// Lerp object transform forward to given first parameter Vector3 position in given second parameter Float duration.
    /// </summary>
    /// <param name="targetPosition">Second parameter for Lerp Function as target lerp position</param>
    /// <param name="duration">Duration it takes to lerp</param>
    /// <returns></returns>
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
