using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEditor;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Photons Lobby callbacks.
/// </summary>
public class LobbyCallbacks : MonoBehaviourPunCallbacks
{

    public delegate void LobbyDelegate();
    public static event LobbyDelegate onSuccessfullyJoinedLobby;

    public override void OnJoinedLobby()
    {
        PlayerInformation.Name = "Anonym " + Random.Range(0,10000);
        PhotonNetwork.LocalPlayer.NickName = PlayerInformation.Name;
        Debug.Log("You entered into the lobby.");
        //Attempts to join random room, TODO: Change this to happen on UI button press.
        //JoinSomeRandomRoom();
        onSuccessfullyJoinedLobby?.Invoke();
    }

    public override void OnLeftLobby()
    {
        Debug.Log("You left from the lobby.");
    }

    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("Lobby statistics updated, new statistics : ");
        if (lobbyStatistics.Count != 0)
        {
            for (int i = 0; i < lobbyStatistics.Count; i++)
            {
                Debug.Log("Lobby Name = " + lobbyStatistics[i].Name + "\nPlayer Count = " + lobbyStatistics[i].PlayerCount + "\nTotal Rooms = " + lobbyStatistics[i].RoomCount + "\nType of Lobby = " + lobbyStatistics[i].Type);
            }
        }
        else
        {
            Debug.Log("There are currently no lobbys...");
        }
    }

    /// <summary>
    /// Updates the listing of rooms when room is created.
    /// 
    /// TODO : Update the rooms into the Lobby UI
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Roomlist Update...");
        if (roomList.Count != 0)
        {
            Debug.Log("Received Roomlist Update : ");
            for (int i = 0; i < roomList.Count; i++)
            {
                Debug.Log("Room Name = " + roomList[i].Name + "\nOpen = " + roomList[i].IsOpen + "\nPlayers = " + roomList[i].PlayerCount + "/" + roomList[i].MaxPlayers);
            }
        }
        else
        {
            Debug.Log("There are currently no Rooms...");
        }
    }
}
