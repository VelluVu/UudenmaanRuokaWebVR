using System.Collections;
using UnityEngine;

public class ActivateTeleportOnHand : MonoBehaviour
{
    public PhysicsRaycast[] raycasters;
    public LineRenderer[] lines;

    public float teleportingTime = 3f;
    public float teleportActivationDelay = 0.5f;

    Coroutine teleportUptime;
    Coroutine activateTeleport;

    void Update()
    {
        if(raycasters[1] == null || raycasters[0] == null)
        {
            Debug.LogError("Raycasters are not set for " + this + ". Please drag and drop the controllers into array.");
            return;
        }

        if (!raycasters[1].active && Input.GetButtonDown("TriggerRight"))
        {
            if (activateTeleport != null)
                StopCoroutine(activateTeleport);
            if(teleportUptime != null)
                StopCoroutine(teleportUptime);

            activateTeleport = StartCoroutine(ActivateTeleport(raycasters[1], raycasters[0], lines[0]));
          
            teleportUptime = StartCoroutine(TeleUp(raycasters[1], lines[1]));
        }

        if(!raycasters[0].active && Input.GetButtonDown("TriggerLeft"))
        {
            if (activateTeleport != null)
                StopCoroutine(activateTeleport);
            if (teleportUptime != null)
                StopCoroutine(teleportUptime);

            activateTeleport = StartCoroutine(ActivateTeleport(raycasters[0], raycasters[1], lines[1]));
           
            teleportUptime = StartCoroutine(TeleUp(raycasters[0], lines[0]));
        }
    }

    IEnumerator ActivateTeleport(PhysicsRaycast caster, PhysicsRaycast previousCaster, LineRenderer line)
    {
        line.positionCount = 0;
        previousCaster.active = false;
        yield return new WaitForSeconds(teleportActivationDelay);
        caster.active = true;
    }

    IEnumerator TeleUp(PhysicsRaycast caster, LineRenderer line)
    {
        yield return new WaitForSeconds(teleportingTime);
        line.positionCount = 0;
        caster.active = false;
    }
}
