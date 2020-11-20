using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class is handling the feet movement of IK Feet
/// </summary>
public class FeetIK : MonoBehaviour
{

    private Animator anim;
    public Vector3 footOffset;
    [Range(0, 1)]
    public float rightFootPosWeight = 1.0f;
    [Range(0, 1)]
    public float rightFootRotWeight = 1.0f;
    [Range(0, 1)]
    public float leftFootPosWeight = 1.0f;
    [Range(0, 1)]
    public float leftFootRotWeight = 1.0f;

    VRRig rig;
    Vector3 previousPos;
    Vector3 currentPos;
    public Vector3 velocityVector;
    public Vector3 headsetLocalSpeed;
    public float interval = 0.1f;
    [Range(0,1)]
    public float smoothing = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<VRRig>();
        //StartCoroutine(UpdateAnimationSpeed());
    }

    private void FixedUpdate()
    {
        currentPos = rig.head.vrTarget.position;
        
        velocityVector = new Vector3((currentPos - previousPos).x / Time.fixedDeltaTime, 0, (currentPos - previousPos).z / Time.fixedDeltaTime);

        headsetLocalSpeed = transform.InverseTransformDirection(velocityVector);

        previousPos = rig.head.vrTarget.position;

        float previousDirX = anim.GetFloat("MovementX");
        float previousDirZ = anim.GetFloat("MovementZ");

        anim.SetFloat("MovementX", Mathf.Lerp(previousDirX, Mathf.Clamp(headsetLocalSpeed.x, -1, 1), smoothing));
        anim.SetFloat("MovementZ", Mathf.Lerp(previousDirZ, Mathf.Clamp(headsetLocalSpeed.z, -1, 1), smoothing));

    }

    //IEnumerator UpdateAnimationSpeed()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(interval);

    //        //Debug.Log("MovementX : " + Mathf.Clamp(headsetLocalSpeed.x, -1, 1));
    //        //Debug.Log("MovementZ : " + Mathf.Clamp(headsetLocalSpeed.z,-1,1));
          
    //    }
    //}

    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 rightFootPos = anim.GetIKPosition(AvatarIKGoal.RightFoot);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(rightFootPos + Vector3.up, Vector3.down, out hit);
        if (hasHit && hit.collider.gameObject.layer != LayerMask.NameToLayer("Controller") && hit.collider.gameObject.layer != LayerMask.NameToLayer("IK_Body"))
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootPosWeight);
            anim.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + footOffset);

            Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotWeight);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, footRotation);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
        }


        Vector3 leftFootPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot);
        hasHit = Physics.Raycast(leftFootPos + Vector3.up, Vector3.down, out hit);

        if (hasHit && hit.collider.gameObject.layer != LayerMask.NameToLayer("Controller") && hit.collider.gameObject.layer != LayerMask.NameToLayer("IK_Body"))
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootPosWeight);
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + footOffset);

            Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotWeight);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, footRotation);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }
}
