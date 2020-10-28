using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is moving the body with player
/// </summary>
public class BodyIK : MonoBehaviour
{
    public Transform vrBody;
    public Transform hmdView;
    public Transform hmd;
    public Transform model;
    public Transform head;
    public Transform hips;
    public float maxPlayerHeight = 1.8f;
    public float playerHeight = 1.8f;
    public float modelHeight = 0f;
    public float hipsY;
    public Vector3 bodyOffset;

    public float heightUpdateRate;

    public delegate void UpdateHeight();
    public static event UpdateHeight updateHeight;

    private void Start()
    {
        modelHeight = hmdView.position.y;
        playerHeight = hmd.position.y - transform.position.y;
        maxPlayerHeight = Mathf.Max(playerHeight / maxPlayerHeight);
        model.localScale = Vector3.one * (maxPlayerHeight / modelHeight);
        hipsY = hips.position.y;
        transform.position = new Vector3(hmd.position.x, hipsY, hmd.position.z);
        StartCoroutine(UpdateHeightTick());
    }

    private void OnEnable()
    {
        updateHeight += PositionBody;
    }

    private void OnDisable()
    {
        updateHeight -= PositionBody; 
    }

    private void FixedUpdate()
    {
       
        transform.rotation = vrBody.rotation;
        transform.position = new Vector3(hmd.position.x, transform.position.y, hmd.position.z);
    }

    void PositionBody()
    {       
        transform.position = new Vector3(hmd.position.x, hipsY, hmd.position.z);
    }

    IEnumerator UpdateHeightTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(heightUpdateRate);
            updateHeight?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(hips.position, new Vector3(transform.position.x, head.position.y, transform.position.z));
        
    }
}
