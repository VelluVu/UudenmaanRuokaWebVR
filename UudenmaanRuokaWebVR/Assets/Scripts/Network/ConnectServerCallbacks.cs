using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Photons server connection callbacks.
/// </summary>
public class ConnectServerCallbacks : MonoBehaviourPunCallbacks
{
    public static bool isConnecting;

    private void Start()
    {
        isConnecting = PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("Established a Low Level Connection to the Photon Server.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the Photon Server and ready for other tasks!");
        PhotonNetwork.JoinLobby();
       
    }

    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("Authentication Failed " + debugMessage);
    }

    public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("Custom authentication response are : ");
        //Find the correct custom message from dictionary.
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from the Photon server because of " + cause);
        isConnecting = false;
    }

    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("Region list received ");
        //Use the parameter to futher exploration
    }
}
