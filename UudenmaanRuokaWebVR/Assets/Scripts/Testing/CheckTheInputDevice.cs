using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Tracks the device currently connected.
/// </summary>
public class CheckTheInputDevice : MonoBehaviour
{

    public InputDevice device;
    public static string currentDevice; //Tarkista inputit? Deactivoi luokkia/gameObjecteja mitä ei voi käyttää?

    private void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        Debug.Log("CONNECTED DEVICE NAME IS : " + device.name);
        currentDevice = device.name;
    }
}
