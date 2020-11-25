using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;

public class BreakHold : MonoBehaviour
{

    public bool isColliding = false;
    PickUpInteraction pickUp;
    Rigidbody rb;
    Vector3 originalPos;
    Quaternion originalRot;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPos = transform.position;
        originalRot = transform.rotation;

        StartCoroutine(ResetPosition());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && pickUp != null)
        {
            isColliding = true;
            pickUp.Drop();
            pickUp = null;           
        }
    }

    public void PickUp(PickUpInteraction pickUpInteraction)
    {
        isColliding = false;
        pickUp = pickUpInteraction;
    }

    public IEnumerator ResetPosition()
    {
        while (true)
        {
            if (transform.position.y < -50f)
            {
                transform.position = originalPos;
                transform.rotation = originalRot;
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
