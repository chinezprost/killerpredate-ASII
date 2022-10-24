using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UIManager : NetworkBehaviour
{
    public GameObject SheriffUI;
    public GameObject KillerUI;
    public GameObject InnocentUI;


    public void PlayerShowUI(int type)
    {
        if (!IsOwner) return;
        
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
