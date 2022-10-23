using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Awake()
    {

        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("Server has been started.");
                NetworkManager.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
            }
        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            //NetworkManager.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        });
    }
}
