using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkVariables : NetworkBehaviour
{
    public struct fixedString : INetworkSerializable
    {
        public FixedString128Bytes _string;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _string);
        }
    }
    
    public NetworkVariable<FixedString128Bytes> readyText = new NetworkVariable<FixedString128Bytes>();
    public NetworkVariable<int> readyPlayers = new NetworkVariable<int>(0);


    public GameObject readyTextGameobject;
    public GameObject readyPlayersGameobject;

    public LobbyNetworkController lobbyNetworkController;

    

    public void InitializeVariables()
    {
        if(SceneManager.GetActiveScene().name == "Lobby")
            readyTextGameobject.GetComponent<TextMesh>().text = $"{readyPlayers.Value} ready players.";
    }
    

    public override void OnNetworkSpawn()
    {
        InitializeVariables();
        
        readyPlayers.OnValueChanged += (int previous, int current) =>
        {
            readyTextGameobject.GetComponent<TextMesh>().text = $"{readyPlayers.Value} ready players.";
            if (NetworkManager.Singleton.IsHost && current == NetworkManager.ConnectedClients.Count)
            {
                Debug.Log("Starting game...");
                lobbyNetworkController.LoadPlayArea();
            }
            Debug.Log("Value changed" + current);
        };

        readyText.OnValueChanged += (FixedString128Bytes previous, FixedString128Bytes current) =>
        {
            
            Debug.Log("String changed" + current);
            readyTextGameobject.GetComponent<TextMesh>().text = current.Value;
        };
    }
}
