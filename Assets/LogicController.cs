using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class LogicController : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        GeneratePlayerClass();
    }


    private void GeneratePlayerClass()
    {
        if (!IsHost) return;
        
        var random = new Unity.Mathematics.Random((uint)DateTime.Now.Millisecond);

        var sheriffId =
            NetworkManager.Singleton.ConnectedClientsList[
                random.NextInt(NetworkManager.Singleton.ConnectedClientsIds.Count)].ClientId;
        
        
        
        var killerId = NetworkManager.Singleton.ConnectedClientsList[
            random.NextInt(NetworkManager.Singleton.ConnectedClientsIds.Count)].ClientId;


        int maxIterations = 8;
        while (sheriffId == killerId && maxIterations != 0)
        {
            killerId = NetworkManager.Singleton.ConnectedClientsList[
                random.NextInt(NetworkManager.Singleton.ConnectedClientsIds.Count)].ClientId;

            maxIterations--;
        }
        
        Debug.Log($"Sheriff: {sheriffId}, Killer: {killerId}");

        foreach (var connectedClientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Debug.Log(connectedClientId);
            var playerData = NetworkManager.SpawnManager.GetPlayerNetworkObject(connectedClientId)
                .GetComponent<PlayerData>();

            if (connectedClientId == sheriffId)
            {
                playerData.playerClass.Value = 2; //sheriff
                ShowPlayerUIClientRpc(2,
                    new ClientRpcParams
                        { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { connectedClientId } } });
            }
            else if (connectedClientId == killerId)
            {
                playerData.playerClass.Value = 3; //killer
                ShowPlayerUIClientRpc(3,
                    new ClientRpcParams
                        { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { connectedClientId } } });
            }
            else
            {
                playerData.playerClass.Value = 1; //innocent
                ShowPlayerUIClientRpc(1,
                    new ClientRpcParams
                        { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { connectedClientId } } });
            }
        }
    }

    [ClientRpc]
    public void ShowPlayerUIClientRpc(int playerClass, ClientRpcParams @params)
    {
        Debug.Log($"Player: {OwnerClientId} has received ClientRPC.");
        var UIManager = NetworkManager.SpawnManager.GetLocalPlayerObject().GetComponent<UIManager>();
        UIManager.PlayerShowUI(playerClass);

    }
}