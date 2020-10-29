using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public Button button_joinRoom;
    public Button button_leaveRoom;

    private void OnEnable()
    {
        RoomCallbacks.onSuccessfullyJoinedRoom += SwapRoomAccessButtons;
        RoomCallbacks.onSuccessfullyLeftRoom += SwapRoomAccessButtons;
        LobbyCallbacks.onSuccessfullyJoinedLobby += EnableJoinRoom;
    }

    private void OnDisable()
    {
        RoomCallbacks.onSuccessfullyJoinedRoom -= SwapRoomAccessButtons;
        RoomCallbacks.onSuccessfullyLeftRoom -= SwapRoomAccessButtons;
        LobbyCallbacks.onSuccessfullyJoinedLobby -= EnableJoinRoom;
    }

    public void SwapRoomAccessButtons()
    {
        if (button_joinRoom.enabled)
        {
            button_joinRoom.gameObject.SetActive(false);
            button_leaveRoom.gameObject.SetActive(true);
        }
        else
        {
            button_joinRoom.gameObject.SetActive(true);
            button_leaveRoom.gameObject.SetActive(false);
        }
    }

    public void EnableJoinRoom()
    {
        button_joinRoom.interactable = true;
    }

}
