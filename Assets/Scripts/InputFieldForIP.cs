using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.TLS;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldForIP : MonoBehaviour
{
    public UnityTransport networkManager;
    public TMP_InputField inputFieldForIPandPort;

    public string IP;
    public int PORT = 25565;

    public void Start()
    {
        inputFieldForIPandPort.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void ValueChangeCheck()
    {
        var pair = inputFieldForIPandPort.text;
        var position = pair.IndexOf(":");
        if (position < 0) return;
        
        
        networkManager.ConnectionData.Address = pair.Substring(0, position);
        if(pair.Substring(position+1).Length > 0)
            networkManager.ConnectionData.Port = Convert.ToUInt16(pair.Substring(position + 1));
    }
}
