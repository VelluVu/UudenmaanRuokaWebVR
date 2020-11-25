using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Handles pick up interactions and other interactions.
/// </summary>
public class PickUpInteraction : MonoBehaviour
{
    private FixedJoint attachJoint = null;
    private Rigidbody currentRigidBody = null;
    private List<Rigidbody> contactRigidBodies = new List<Rigidbody>();

    private Transform t;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    public Collider IKHandCollider;

    private Animator anim;
    private WebXRController controller;
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
        teleport = GetComponent<PhysicsRaycaster>();
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<WebXRController>();
    }

    void Update()
    {
        if (teleport.active) return;
        if (distantPickup) return;
        if (VRInputModule.pointingUI) return;

        controller.TryUpdateButtons();

        if (controller.GetButtonDown(WebXRController.ButtonTypes.Trigger)
            || controller.GetButtonDown(WebXRController.ButtonTypes.Grip)
            || controller.GetButtonDown(WebXRController.ButtonTypes.ButtonA))
        {
            Pickup();
        }

        if (controller.GetButtonUp(WebXRController.ButtonTypes.Trigger)
            || controller.GetButtonUp(WebXRController.ButtonTypes.Grip)
            || controller.GetButtonUp(WebXRController.ButtonTypes.ButtonA))
        {
            Drop();
        }

    }

    private void FixedUpdate()
    {
        if (!currentRigidBody) return;

        lastPosition = currentRigidBody.position;
        lastRotation = currentRigidBody.rotation;

        if(currentRigidBody.gameObject.layer == LayerMask.NameToLayer("Handler"))
        {
            //Debug.Log("CURRENT RIGIDBODY IS DOOR HANDLER");
            if (Vector3.Distance(currentRigidBody.transform.parent.position, transform.position) > 0.5f)
            {
                //Debug.Log("TOO FAR");
                Drop();
            }
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Interactable")
            return;

        teleport.active = false;

        contactRigidBodies.Add(other.gameObject.GetComponent<Rigidbody>());
        controller.Pulse(0.5f, 250);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Interactable")
            return;

        contactRigidBodies.Remove(other.gameObject.GetComponent<Rigidbody>());

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

        BreakHold breakHold = currentRigidBody.GetComponent<BreakHold>();
        if (breakHold)
            breakHold.PickUp(this);

        IKHandCollider.isTrigger = true;
        if (currentRigidBody.gameObject.layer == LayerMask.NameToLayer("Handler"))
        {
            
            currentRigidBody.isKinematic = false;
            currentRigidBody.transform.parent.GetComponent<DoorHandler>().stop = false;
        }
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

            IKHandCollider.isTrigger = true;
            StartCoroutine(Magnetic(other.transform.position, other.attachedRigidbody));
        }

        if (currentRigidBody)
        {
            BreakHold breakHold = currentRigidBody.GetComponent<BreakHold>();
            if (breakHold)
                breakHold.PickUp(this);
            lastPosition = currentRigidBody.position;
            lastRotation = currentRigidBody.rotation;
        }
    }

    IEnumerator Magnetic(Vector3 startPos, Rigidbody other)
    {
        float elapsedTime = 0;
        while ((elapsedTime < time) && distantPickup)
        {
            //Debug.Log(" Magnetismmmmm : " + elapsedTime);
            controller.Pulse(0.25f, 150);
            other.position = Vector3.Lerp(startPos, transform.position, elapsedTime / time);
            elapsedTime += Time.deltaTime;

            //if(Vector3.Distance(other.transform.position, transform.position) < 0.01f)
            //{
            //    currentRigidBody.position = other.position;
            //    attachJoint.connectedBody = currentRigidBody;
            //}

            yield return null;
        }
        attachJoint.connectedBody = currentRigidBody;
    }

    public void Drop()
    {

        distantPickup = false;
        initPickUp = false;
        attachJoint.connectedBody = null;

        if (currentRigidBody)
        {
            if(currentRigidBody.gameObject.layer == LayerMask.NameToLayer("Handler"))
            {
                currentRigidBody.transform.parent.GetComponent<DoorHandler>().stop = true;
                currentRigidBody.transform.localPosition = Vector3.zero;
                currentRigidBody.transform.localRotation = Quaternion.Euler(Vector3.zero);
                currentRigidBody.transform.localScale = Vector3.one;
                currentRigidBody.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
                currentRigidBody.transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                currentRigidBody.transform.parent.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
                currentRigidBody.transform.parent.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;      
                currentRigidBody.isKinematic = true;
                currentRigidBody = null;
                IKHandCollider.isTrigger = false;
                teleport.active = true;
                return;
            }

            currentRigidBody.velocity = (currentRigidBody.position - lastPosition) / Time.deltaTime;

            var deltaRotation = currentRigidBody.rotation * Quaternion.Inverse(lastRotation);
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            angle *= Mathf.Deg2Rad;
            currentRigidBody.angularVelocity = axis * angle / Time.deltaTime;

            currentRigidBody = null;
        }

        teleport.active = true;
        IKHandCollider.isTrigger = false;
    }

    private Rigidbody GetNearestRigidBody()
    {
        Rigidbody nearestRigidBody = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Rigidbody contactBody in contactRigidBodies)
        {
            distance = (contactBody.gameObject.transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestRigidBody = contactBody;
            }
        }

        return nearestRigidBody;
    }
}
