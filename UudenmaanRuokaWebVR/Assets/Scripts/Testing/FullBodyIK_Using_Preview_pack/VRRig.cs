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

    // Start is called before the first frame update
    void Awake()
    {
        headBodyOffset = transform.position - headConstraint.position;    
    }
   
    // Update is called once per frame
    void FixedUpdate()
    {        
        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = new Vector3(headConstraint.forward.x, transform.forward.y, headConstraint.forward.z);
        
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
