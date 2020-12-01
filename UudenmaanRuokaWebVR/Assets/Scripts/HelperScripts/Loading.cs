using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Loads necessary scenes and does some other scene loading aswell
/// </summary>
public class Loading : MonoBehaviour
{
    /// <summary>
    /// Are all necessary components loaded, player, network, lobby.
    /// </summary>
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
        //RoomCallbacks.onSuccessfullyLeftRoom += LeaveRoom;
        RoomCallbacks.onSuccessfullyJoinedRoom += LoadLevel;
        LobbyUI.onJoinRandomRoom += JoinSomeRandomRoom;
    }
    private void OnDisable()
    {
        //RoomCallbacks.onSuccessfullyLeftRoom -= LeaveRoom;
        RoomCallbacks.onSuccessfullyJoinedRoom -= LoadLevel;
        LobbyUI.onJoinRandomRoom -= JoinSomeRandomRoom;
    }

    /// <summary>
    /// Loads the player scene
    /// </summary>
    public void LoadPlayer()
    {
        SceneManager.LoadScene("PlayerScene", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Loads the network scene
    /// </summary>
    public void LoadNetwork()
    {
        SceneManager.LoadSceneAsync("NetworkScene", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Loads the lobby scene
    /// </summary>
    public void LoadLobby()
    {
        SceneManager.LoadSceneAsync("LobbyScene", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Leaves the Room
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// Loads the Room Scene
    /// </summary>
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
