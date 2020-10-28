using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Using unity mechanim IK for Head.
/// </summary>
public class HeadIK : MonoBehaviour
{
    public Transform hmd;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float reach = anim.GetFloat("HeadWeight");
        anim.SetLookAtWeight(reach);
        anim.SetLookAtPosition(hmd.position + hmd.forward);
    }
}
