using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollisionCheck : MonoBehaviour
{

    public Transform rig;
    public Transform body;
    public Transform[] cams;
    public Transform feet;

    private void Update()
    {
#if UNITY_EDITOR
        transform.position = new Vector3(cams[0].position.x, feet.position.y, cams[0].position.z);
#elif !UNITY_EDITOR && UNITY_WEBGL
        transform.position = new Vector3(cams[1].position.x, feet.position.y, cams[1].position.z);
#endif
    }

}
