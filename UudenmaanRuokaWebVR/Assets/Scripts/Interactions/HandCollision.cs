using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollision : MonoBehaviour
{
    Vector3 collisionPoint;
    Vector3 hitNormal;
    Vector3 calculated;
    public float multiplier = 0.02f;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") || collision.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            //Debug.Log("Hitting body to Obstacle " + collision.collider.name);
            collisionPoint = collision.GetContact(0).point;
            hitNormal = collision.GetContact(0).normal;
            Vector3 hitNormalFlatY = new Vector3(hitNormal.x, 0, hitNormal.z);

            Debug.DrawRay(collisionPoint, hitNormalFlatY, Color.red, 5f);
            //Debug.Log("calculated " + hitNormalFlatY);
            calculated = hitNormalFlatY * multiplier;
            transform.position = calculated;
        }
    }
}
