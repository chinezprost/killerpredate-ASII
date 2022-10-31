using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class UIManager : NetworkBehaviour
{
    public GameObject SheriffUI;
    public GameObject KillerUI;
    public GameObject InnocentUI;

    public GameObject PlayerTimer;

    public GameObject KillGameObject;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;
        
        KillGameObject.SetActive(true);
    }

    public void UpdatePlayerTimer(int minutes, int seconds)
    {
        if (!IsOwner) return;
        
        var minutesToShow = (minutes < 10) ? $"0{minutes}" : minutes.ToString();
        var secondsToShow = (seconds < 10) ? $"0{seconds}" : seconds.ToString();
        
        PlayerTimer.GetComponentInChildren<TMP_Text>().text = $"{minutesToShow}:{secondsToShow}";
    }

    public void PlayerShowUI(int type)
    {
        if (!IsOwner) return;
        
        PlayerTimer.SetActive(true);
        switch (type)
        {
            case 1: 
                //innocent;
                InnocentUI.SetActive(true);
                break;
            case 2:
                //sheriff;
                SheriffUI.SetActive(true);
                break;
            case 3:
                //killer;
                KillerUI.SetActive(true);
                break;
        }
    }
}
