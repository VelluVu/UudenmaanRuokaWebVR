using UnityEngine;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Checks Head collision and does something TODO...
/// </summary>
public class HeadCollisionCheck : MonoBehaviour
{
    public Transform rig;
    public Transform body;
    public Transform[] cams;



    Vector3 collisionPoint;
    Vector3 hitNormal;
    Vector3 calculated;
    public float multiplier = 0.2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") || collision.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            //Debug.Log("Hitting head to Obstacle " + collision.collider.name);
            collisionPoint = collision.GetContact(0).point;
            hitNormal = collision.GetContact(0).normal;
            Vector3 hitNormalFlatY = new Vector3(hitNormal.x, 0, hitNormal.z);

            Debug.DrawRay(collisionPoint, hitNormalFlatY, Color.red, 5f);
            //Debug.Log("calculated " + hitNormalFlatY);
            calculated = hitNormalFlatY * multiplier;
            rig.position += calculated;
            body.position += calculated;
            //for (int i = 0; i < cams.Length; i++)
            //{
            //    cams[i].position = calculated;
            //}
        }
    }

}
