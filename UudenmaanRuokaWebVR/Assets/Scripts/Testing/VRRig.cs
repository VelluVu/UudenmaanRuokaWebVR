using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

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

public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    //float height;
    public Transform controller;
    //public Transform body;
    public Transform headConstraint;
    public Vector3 headBodyOffset;
 
    // Start is called before the first frame update
    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
        //height = -headConstraint.position.y*0.4f;
        //head.trackingPositionOffset.y = height;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(head.vrTarget.position.x, head.vrTarget.position.y * 0.6f, head.vrTarget.position.z ) - controller.forward * 0.1f;
        transform.rotation = controller.rotation;
       
        //transform.forward = Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized;
        //transform.position = headConstraint.position + headBodyOffset;
        //transform.forward = Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized;

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
