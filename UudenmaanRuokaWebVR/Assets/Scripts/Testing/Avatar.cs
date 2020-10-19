using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    public Transform leftController;
    public Transform rightController;

    private void Update()
    {
        leftHand.position = leftController.position;
        leftHand.rotation = leftController.rotation;
        rightHand.position = rightController.position;
        rightHand.rotation = rightController.rotation;
    }

}
