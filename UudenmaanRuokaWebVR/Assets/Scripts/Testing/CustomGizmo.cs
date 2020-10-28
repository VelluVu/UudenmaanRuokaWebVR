using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Add gizmo for object
/// </summary>
public class CustomGizmo : MonoBehaviour
{
    public Color gizmoColor = Color.red;
    public GizmoDrawType type;
    public float sphereRadius = 0.05f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        switch (type)
        {         
            case GizmoDrawType.Sphere:
                Gizmos.DrawSphere(transform.position, sphereRadius);
                break;
            case GizmoDrawType.WireSphere:
                Gizmos.DrawWireSphere(transform.position, sphereRadius);
                break;
            default:
                
                break;
        }
    }

}

public enum GizmoDrawType
{
    Sphere,
    WireSphere

}
