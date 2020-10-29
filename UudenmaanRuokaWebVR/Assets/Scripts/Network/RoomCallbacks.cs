using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Photons room Callbacks and creation of default room.
/// </summary>
public class RoomCallbacks : MonoBehaviourPunCallbacks
{

    public string firstRoomName = "Eka Huone";
    RoomOptions defaultRoomOptions;

    public delegate void RoomDelegate();
    public static event RoomDelegate onSuccessfullyJoinedRoom;
    public static event RoomDelegate onSuccessfullyLeftRoom;

    public override void OnCreatedRoom()
    {
        Debug.Log("Created The Room " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {     
        Debug.Log("Creating the Room failed " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " Joined in to the Room " + PhotonNetwork.CurrentRoom.Name);
        onSuccessfullyJoinedRoom?.Invoke();

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(PhotonNetwork.IsMasterClient);
            LoadLevel();
        }
    }

    /// <summary>
    /// Change these if want to alter default room options.
    /// </summary>
    /// <returns></returns>
    public RoomOptions CreateDefaultRoomOptions()
    {
        defaultRoomOptions = new RoomOptions();
        defaultRoomOptions.MaxPlayers = 5;
        defaultRoomOptions.IsOpen = true;
        defaultRoomOptions.IsVisible = true;
        return defaultRoomOptions;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Joining random Room failed " + message);

        PhotonNetwork.JoinOrCreateRoom(firstRoomName, CreateDefaultRoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Joining Room failed " + message);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the Room");
        SceneManager.LoadScene(0);
        onSuccessfullyLeftRoom?.Invoke();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered the Room " + newPlayer.NickName);

        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log(PhotonNetwork.IsMasterClient);
            LoadLevel();
        }

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left the Room " + otherPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(PhotonNetwork.IsMasterClient);
            LoadLevel();
        }
    }

    public void LoadLevel()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Trying to load level but we are not masterclient!");
        }
        Debug.Log("Loading level " + PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel(1);
        
    }
}
