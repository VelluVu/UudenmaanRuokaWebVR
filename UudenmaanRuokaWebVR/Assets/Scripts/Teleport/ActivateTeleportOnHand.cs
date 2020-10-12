using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using WebXR;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Changes the teleporting hand on trigger press 
/// and 
/// delays the teleport trigger 
/// so 
/// player doesn't instantly/accidently be able to teleport while aiming the Teleport area.  
/// 
/// This script no longer used , somehow not working in webgl.
/// </summary>
public class ActivateTeleportOnHand : MonoBehaviour
{
    public bool debugging = true;
    public PhysicsRaycaster[] raycasters;
    //public LineRenderer[] lines;
    public WebXRController[] controllers;
    public float teleportActivationDelay = 0.5f;

    Coroutine activateTeleport;

    /// <summary>
    /// Tries to automatically find the classes reguired for this to function.
    /// </summary>
    private void Start()
    {
        
        if (raycasters[1] == null || raycasters[0] == null)
        {
            raycasters[0] = transform.GetChild(0).gameObject.GetComponent<PhysicsRaycaster>();
            raycasters[1] = transform.GetChild(1).gameObject.GetComponent<PhysicsRaycaster>();

            if (raycasters[1] == null || raycasters[0] == null)
            {
                if (debugging)
                    Debug.LogError("Raycasters are not set for " + this + ". Please drag and drop the controllers into array.");
                return;
            }

            if (controllers[0] == null || controllers[1] == null)
            {
                controllers[0] = raycasters[0].GetComponent<WebXRController>();
                controllers[1] = raycasters[1].GetComponent<WebXRController>();

                if (controllers[0] == null || controllers[1] == null)
                {
                    if (debugging)
                        Debug.LogError("Controllers are not set for " + this + ". Please drag and drop the controllers into array.");
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Shoots the ray and if collides Teleport layer player can activate the teleport hand with Trigger.
    /// </summary>
    void Update()
    {

        /*if (!XRDevice.isPresent)
            return;
        */
        if (Physics.Raycast(raycasters[1].transform.position, raycasters[1].transform.forward, raycasters[1].maxDistance, raycasters[1].hitLayer))
        {
            if (!raycasters[1].active && (controllers[1].GetButtonDown("Trigger") || controllers[1].GetButtonDown("Grip")))
            {
                this.RestartCoroutine(ActivateTeleport(raycasters[1], raycasters[0], teleportActivationDelay), ref activateTeleport);
            }
        }
        if (Physics.Raycast(raycasters[0].transform.position, raycasters[0].transform.forward, raycasters[0].maxDistance, raycasters[0].hitLayer))
        {
            if (!raycasters[0].active && (controllers[0].GetButtonDown("Trigger") || controllers[1].GetButtonDown("Grip")))
            {
                this.RestartCoroutine(ActivateTeleport(raycasters[0], raycasters[1], teleportActivationDelay), ref activateTeleport);
            }
        }
    }


    /// <summary>
    /// Deactivates the previous teleport hand and activates the new teleport hand after some time.
    /// </summary>
    /// <param name="caster">new hand to activate</param>
    /// <param name="previousCaster">hand that was previously active</param>
    /// <param name="time">time to wait till the teleport activation</param>
    /// <returns></returns>
    IEnumerator ActivateTeleport(PhysicsRaycaster caster, PhysicsRaycaster previousCaster, float time)
    {
        //line.positionCount = 0;
        previousCaster.active = false;
        yield return new WaitForSeconds(time);
        caster.active = true;
    }

}
