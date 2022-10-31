using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> playerUsername = new NetworkVariable<FixedString64Bytes>("undefined", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> playerClass = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public NetworkVariable<bool> isPlayerAlive = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        isPlayerAlive.OnValueChanged += (bool oldValue, bool newValue) =>
        {
            
        };
    }

    public void OnPlayerConnected(string[] parms)
    {
        if (!IsOwner) return;
        
        InitializePlayerData(parms[0]);
    }


    public void InitializePlayerData(FixedString64Bytes username)
    {
        playerUsername.Value = username;
    }
    
}
