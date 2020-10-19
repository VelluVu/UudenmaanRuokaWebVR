using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Righeight : MonoBehaviour
{
    public Transform rig;
    public Transform cam;
    public float height;

    private void Start()
    {
        height = -1.80f;
    }

    private void Update()
    {
        rig.transform.position = new Vector3(rig.transform.position.x, height + cam.position.y , rig.transform.position.z);
    }
}
