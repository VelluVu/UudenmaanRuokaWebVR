using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFeetIK : MonoBehaviour
{

    Animator anim;
    public Transform target;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float reach = anim.GetFloat("LeftFeetWeight");
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, reach);
        anim.SetIKPosition(AvatarIKGoal.LeftFoot, target.position);
    }
}
