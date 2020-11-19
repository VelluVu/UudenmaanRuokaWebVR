using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    
    public Button joinRandomRoom_Button;

    public delegate void LobbyUIDelegate();
    public static event LobbyUIDelegate onJoinRandomRoom;

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

    public void PressJoinRandomRoom()
    {
        joinRandomRoom_Button.interactable = false;
        onJoinRandomRoom?.Invoke();   
    }

    public void MakeInteractable()
    {
        joinRandomRoom_Button.interactable = true;
    }
}
