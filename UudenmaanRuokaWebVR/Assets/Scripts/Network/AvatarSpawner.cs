using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Spawns the avatar
/// </summary>
public class AvatarSpawner : MonoBehaviourPun
{

    public GameObject networkAvatarPrefab;
    public GameObject avatar;

    public void OnEnable()
    {
        RoomCallbacks.onSuccessfullyJoinedRoom += SpawnAvatar;
        //RoomCallbacks.onSuccessfullyLeftRoom += LeftRoom;
    }

    private void OnDisable()
    {
        RoomCallbacks.onSuccessfullyJoinedRoom -= SpawnAvatar;
        //RoomCallbacks.onSuccessfullyLeftRoom -= LeftRoom;
    }

    public void SpawnAvatar()
    {
        Debug.Log("Spawning " + PlayerInformation.Name + " Network Avatar!");
        avatar = PhotonNetwork.Instantiate(Path.Combine("Prefabs",networkAvatarPrefab.name), Vector3.zero, Quaternion.identity);      
        avatar.GetComponent<NetworkAvatar>().InitAvatar();
    }

    public void LeftRoom()
    {
        Debug.Log("Destroying " + PlayerInformation.Name + " Network Avatar!");
        avatar.GetComponent<NetworkAvatar>().Kill();
    }

}
