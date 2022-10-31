using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mono.CSharp;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TMP_InputField usernameField;
    
    public Canvas canvasGameobject;

    private PlayerData playerData;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            if (usernameField.text == "")
            {
                Debug.Log("Username can't be empty.");
                return;
            }
            NetworkManager.Singleton.StartHost();
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("Server has been started.");
                playerData = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<PlayerData>();
                playerData.OnPlayerConnected(new string[] {usernameField.text});
                NetworkManager.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
            }
        });
        clientButton.onClick.AddListener(async() =>
        {
            if (usernameField.text == "")
            {
                Debug.Log("Username can't be empty.");
                return;
            }
            DontDestroyOnLoad(canvasGameobject.transform.gameObject);
            NetworkManager.Singleton.StartClient();
            StartCoroutine(OnClientConnected());




            //NetworkManager.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        });
    }

    

    public IEnumerator OnClientConnected()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject() != null);
        Debug.Log(NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject());
        playerData = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<PlayerData>();
        playerData.OnPlayerConnected(new string[] { usernameField.text });
        Destroy(canvasGameobject.transform.gameObject);
    }
}
