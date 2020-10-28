using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArmIK : MonoBehaviour
{
    public Transform rightVRController;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float reach = animator.GetFloat("RightHandWeight");
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightVRController.position);
    }

}
