using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

public class PickUpInteraction : MonoBehaviour
{
    private FixedJoint attachJoint = null;
    private Rigidbody currentRigidBody = null;
    private List<Rigidbody> contactRigidBodies = new List<Rigidbody>();

    private Transform t;
    private Vector3 lastPosition;
    private Quaternion lastRotation;

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
