using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class KillPlayerUI : NetworkBehaviour
{

    
    public GameObject killPlayerGameobject;
    public TMP_Text killPlayerText;
    public PlayerNetwork playerNetwork;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;
        
        killPlayerGameobject.SetActive(true);
        playerNetwork = GetComponent<PlayerNetwork>();
    }

    public void UpdateUI()
    {
        killPlayerText.text = (playerNetwork.playerInViewUsername == "none") ? "" : $"Press F to kill {playerNetwork.playerInViewUsername}.";
    }
    
    public void Update()
    {
        if(!IsOwner)
            return;
        UpdateUI();
    }

    
}
