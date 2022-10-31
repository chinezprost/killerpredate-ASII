using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class LobbyNetworkController : NetworkBehaviour
{

    public NetworkVariables networkVariables;
    public Coroutine countdownCorountine;
    private bool isCountdownRunning = false;
    public void LoadPlayArea()
    {
        if (!NetworkManager.Singleton.IsHost)
            return;
        //networkVariables.readyText.Value = $"Starting game... 5 seconds.";
        if(!isCountdownRunning)
            countdownCorountine = StartCoroutine(StartingGameCountdown());
    }

    public void Update()
    {
        if (NetworkManager.Singleton.IsHost && countdownCorountine != null && networkVariables.readyPlayers.Value < NetworkManager.ConnectedClients.Count)
        {
            isCountdownRunning = false;
            StopCoroutine(countdownCorountine);
        }
    }

    public IEnumerator StartingGameCountdown()
    {
        isCountdownRunning = true;
        for (int i = 5; i >= 1; i--)
        {
            networkVariables.readyText.Value = $"Starting game... {i} seconds.";
            yield return new WaitForSeconds(1f);
            
            
        }
        
        NetworkManager.SceneManager.LoadScene("PlayArea", LoadSceneMode.Single);
        isCountdownRunning = false;
    }

    
    
    
}
