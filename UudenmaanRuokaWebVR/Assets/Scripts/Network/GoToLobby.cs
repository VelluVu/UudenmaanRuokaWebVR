using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Handles player input to move back to lobby.
/// </summary>
public class GoToLobby : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            GoLobby();
        }
    }

    public void GoLobby()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene("LobbyScene");
    }

}
