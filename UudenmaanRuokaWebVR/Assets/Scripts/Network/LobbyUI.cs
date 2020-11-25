using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Handles the lobby room buttons or button. TODO : Room list and manual joining-/create room.
/// </summary>
public class LobbyUI : MonoBehaviour
{
    
    public Button joinRandomRoom_Button;

    public delegate void LobbyUIDelegate();
    public static event LobbyUIDelegate onJoinRandomRoom;

    bool connectedAndReady = false;

    private void Start()
    {
        joinRandomRoom_Button.interactable = false;
    }

    private void OnEnable()
    {
        LobbyCallbacks.onSuccessfullyJoinedLobby += MakeInteractable;
    }

    private void OnDisable()
    {
        LobbyCallbacks.onSuccessfullyJoinedLobby -= MakeInteractable;
    }

    private void Update()
    {
        if (connectedAndReady)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PressJoinRandomRoom();
            }
        }
    }

    public void PressJoinRandomRoom()
    {
        connectedAndReady = false;
        joinRandomRoom_Button.interactable = false;
        onJoinRandomRoom?.Invoke();
    }

    public void MakeInteractable()
    {
        joinRandomRoom_Button.interactable = true;
        connectedAndReady = true;
    }
}
