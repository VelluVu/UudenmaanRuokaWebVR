using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmIK : MonoBehaviour
{
    public Transform leftVRController;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float reach = animator.GetFloat("LeftHandWeight");
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reach);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftVRController.position);
    }

}
