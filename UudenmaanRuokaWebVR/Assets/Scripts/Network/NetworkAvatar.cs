using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class NetworkAvatar : MonoBehaviourPun, IPunObservable
{
    public Transform editorHMDTarget;
    public Transform webGLHMDTarget;

    public Transform playerRoot;
    public Transform vrHMDTarget;
    public Transform leftVRHandTarget;
    public Transform rightVRHandTarget;

    public Transform avatarHead;
    public Transform avatarLHand;
    public Transform avatarRHand;
    public Transform avatarBody;

    public TextMeshProUGUI nameText;

    bool init = false;

    public void InitAvatar()
    {
        playerRoot = GameObject.FindGameObjectWithTag("Player").transform;
        editorHMDTarget = playerRoot.GetChild(2).GetChild(0);
        webGLHMDTarget = playerRoot.GetChild(2).GetChild(1);
        leftVRHandTarget = playerRoot.GetChild(0);
        rightVRHandTarget = playerRoot.GetChild(1);

        if(playerRoot == null || editorHMDTarget == null || webGLHMDTarget == null || leftVRHandTarget == null || rightVRHandTarget == null)
        {
            Debug.LogError("Unable to initialize one of " + gameObject.name + " target transforms !");
        }

#if !UNITY_EDITOR && UNITY_WEBGL
        vrHMDTarget = webGLHMDTarget;
#elif UNITY_EDITOR
        vrHMDTarget = editorHMDTarget;
#endif

        nameText.text = PlayerInformation.Name;

        init = true;
    }

    private void FixedUpdate()
    {
        if (init)
        {          
            avatarHead.SetPositionAndRotation(vrHMDTarget.position, vrHMDTarget.rotation);
            avatarLHand.SetPositionAndRotation(leftVRHandTarget.position, leftVRHandTarget.rotation);
            avatarRHand.SetPositionAndRotation(rightVRHandTarget.position, rightVRHandTarget.rotation);
            avatarBody.rotation = Quaternion.Euler(new Vector3(avatarBody.rotation.x, avatarHead.rotation.y, avatarBody.rotation.z));
            transform.position = new Vector3(avatarHead.position.x, 0, avatarHead.position.z);
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (init)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(avatarHead.position);
                stream.SendNext(avatarHead.rotation);
                stream.SendNext(avatarLHand.position);
                stream.SendNext(avatarLHand.rotation);
                stream.SendNext(avatarRHand.position);
                stream.SendNext(avatarRHand.rotation);
                stream.SendNext(avatarBody.position);
                stream.SendNext(avatarBody.rotation);
            }
            else
            {
                transform.position = (Vector3)stream.ReceiveNext();
                avatarHead.position = (Vector3)stream.ReceiveNext();
                avatarHead.rotation = (Quaternion)stream.ReceiveNext();
                avatarLHand.position = (Vector3)stream.ReceiveNext();
                avatarLHand.rotation = (Quaternion)stream.ReceiveNext();
                avatarRHand.position = (Vector3)stream.ReceiveNext();
                avatarRHand.rotation = (Quaternion)stream.ReceiveNext();
                avatarBody.position = (Vector3)stream.ReceiveNext();
                avatarBody.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
