using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightFeetIK : MonoBehaviour
{
    Animator anim;
    public Transform target;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float reach = anim.GetFloat("RightFeetWeight");
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, reach);
        anim.SetIKPosition(AvatarIKGoal.RightFoot, target.position);
    }
}
