using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public static bool LoadedNecessaryComponents = false;
    public void Awake()
    {

        Debug.Log("Already loaded " + LoadedNecessaryComponents);

        if (!LoadedNecessaryComponents)
        {
            LoadPlayer();
            LoadNetwork();
            LoadLobby();

            LoadedNecessaryComponents = true;
        }
    }

    private void OnEnable()
    {
        RoomCallbacks.onSuccessfullyLeftRoom += LeaveRoom;
        RoomCallbacks.onSuccessfullyJoinedRoom += LoadLevel;
        LobbyUI.onJoinRandomRoom += JoinSomeRandomRoom;
    }
    private void OnDisable()
    {
        RoomCallbacks.onSuccessfullyLeftRoom -= LeaveRoom;
        RoomCallbacks.onSuccessfullyJoinedRoom -= LoadLevel;
        LobbyUI.onJoinRandomRoom -= JoinSomeRandomRoom;
    }

    public void LoadPlayer()
    {
        SceneManager.LoadScene("PlayerScene", LoadSceneMode.Additive);
    }

    public void LoadNetwork()
    {
        SceneManager.LoadSceneAsync("NetworkScene", LoadSceneMode.Additive);
    }

    public void LoadLobby()
    {
        SceneManager.LoadSceneAsync("LobbyScene", LoadSceneMode.Additive);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void LoadLevel()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("First player joined the room, loading level.");
            PhotonNetwork.LoadLevel(4);
        }
    }

    /// <summary>
    /// Joins random room, if fails triggers the on join random room failed callback.
    /// </summary>
    public void JoinSomeRandomRoom()
    {
        if (ConnectServerCallbacks.isConnecting)
        {
            Debug.Log("Joining Random Room");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            ConnectServerCallbacks.isConnecting = PhotonNetwork.ConnectUsingSettings();
        }
    }
}
