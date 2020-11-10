using UnityEngine;
using System.Collections.Generic;
using WebXR;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FixedJoint))]
[RequireComponent(typeof(WebXRController))]
public class DesertControllerInteraction : MonoBehaviour
{
    private FixedJoint attachJoint;
    private Rigidbody currentRigidBody;
    private List<Rigidbody> contactRigidBodies = new List<Rigidbody>();
    private WebXRController controller;
    private Transform t;
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    //private Animator anim;
    private PhysicsRaycaster teleport;
    public int interpolationFramesCount = 45;
    int elapsedFrames = 0;

    public bool distantPickup = false;
    public bool initPickUp = false;
    float time = 1f;

    void Awake()
    {
        t = transform;
        attachJoint = GetComponent<FixedJoint>();
        //anim = GetComponent<Animator>();
        controller = GetComponent<WebXRController>();
        teleport = GetComponent<PhysicsRaycaster>();
    }

    void Update()
    {
        if (teleport.active) return; // Changed this for teleporting
        if (distantPickup) return;

        if (controller.GetButtonDown("Trigger") || controller.GetButtonDown("Grip"))
            Pickup();

        if (controller.GetButtonUp("Trigger") || controller.GetButtonUp("Grip"))
            Drop();

    }

    void FixedUpdate()
    {
        if (!currentRigidBody) return;

        lastPosition = currentRigidBody.position;
        lastRotation = currentRigidBody.rotation;

    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        teleport.active = false;

        contactRigidBodies.Add(other.attachedRigidbody);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        contactRigidBodies.Remove(other.attachedRigidbody);

        teleport.active = true;
    }

    public bool HoldingObj()
    {
        return currentRigidBody;
    }

    public void Pickup()
    {
        currentRigidBody = GetNearestRigidBody();

        if (!currentRigidBody)
            return;

        currentRigidBody.MovePosition(t.position);
        attachJoint.connectedBody = currentRigidBody;

        lastPosition = currentRigidBody.position;
        lastRotation = currentRigidBody.rotation;
    }

    /// <summary>
    /// Picks up object from distance
    /// </summary>
    /// <param name="other">targeted object collider</param>
    public void DistantPickUp(Vector3 hitpoint, Collider other)
    {
        teleport.active = false;
        distantPickup = true;

        if (!initPickUp)
        {
            initPickUp = true;

            currentRigidBody = other.attachedRigidbody;

            if (!currentRigidBody)
                return;
  
            StartCoroutine(Magnetic(other.transform.position, other.attachedRigidbody));
        }

        if (currentRigidBody)
        {
            lastPosition = currentRigidBody.position;
            lastRotation = currentRigidBody.rotation;
        }
    }

    IEnumerator Magnetic(Vector3 startPos, Rigidbody other)
    {
        float elapsedTime = 0;
        while ((elapsedTime < time) && distantPickup)
        {
            Debug.Log(" Magnetismmmmm");

            other.position = Vector3.Lerp(startPos, transform.position, elapsedTime / time);
            elapsedTime += Time.deltaTime;

            //if(Vector3.Distance(other.transform.position, transform.position) < 0.01f)
            //{
            //    currentRigidBody.position = other.position;
            //    attachJoint.connectedBody = currentRigidBody;
            //}

            yield return null;
        }
        attachJoint.connectedBody = other;
    }

    public void Drop()
    {
        if (!currentRigidBody)
            return;

        if (distantPickup)
            distantPickup = false;

        if (initPickUp)
            initPickUp = false;

        attachJoint.connectedBody = null;

        currentRigidBody.velocity = (currentRigidBody.position - lastPosition) / Time.deltaTime;

        var deltaRotation = currentRigidBody.rotation * Quaternion.Inverse(lastRotation);
        float angle;
        Vector3 axis;
        deltaRotation.ToAngleAxis(out angle, out axis);
        angle *= Mathf.Deg2Rad;
        currentRigidBody.angularVelocity = axis * angle / Time.deltaTime;

        currentRigidBody = null;

        teleport.active = true;
    }

    private Rigidbody GetNearestRigidBody()
    {
        Rigidbody nearestRigidBody = null;
        float minDistance = float.MaxValue;
        float distance;

        foreach (Rigidbody contactBody in contactRigidBodies)
        {
            distance = (contactBody.transform.position - t.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestRigidBody = contactBody;
            }
        }

        return nearestRigidBody;
    }
}
