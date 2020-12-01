using UnityEngine;
using TMPro;
using Photon.Pun;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Sends Network Avatar positions for other users
/// </summary>
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

    /// <summary>
    /// Find transform references and set up the correct camera etc...
    /// ! FOUND NOT WORKIG IN WEBGL DIFFERENT HIERARCHY THERE !
    /// TODO: FIGURE OUT WEBGL HIERARCHY FOR CAMERA SET AND FIX!
    /// </summary>
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

    /// <summary>
    /// Updates network avatar positions to match the  vr targets positions
    /// </summary>
    private void FixedUpdate()
    {
        if (init) // if we are ready to update positions
        {
            transform.position = new Vector3(avatarHead.position.x, 0, avatarHead.position.z);
            avatarHead.SetPositionAndRotation(vrHMDTarget.position, vrHMDTarget.rotation);
            avatarLHand.SetPositionAndRotation(leftVRHandTarget.position, leftVRHandTarget.rotation);
            avatarRHand.SetPositionAndRotation(rightVRHandTarget.position, rightVRHandTarget.rotation);
            avatarBody.rotation = Quaternion.Euler(new Vector3(avatarBody.rotation.x, avatarHead.rotation.y, avatarBody.rotation.z));
        }
    }

    /// <summary>
    /// Destroy this avatar
    /// </summary>
    public void Kill()
    {
        Destroy(gameObject);
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (init) //if we are ready to stream data
        {
            // Sends the position and rotation data through network
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
            } // Receives and casts the network data if reading
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
