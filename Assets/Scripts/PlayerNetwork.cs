using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;




public class PlayerNetwork : NetworkBehaviour
{
    private bool objectInView = false;
    public List<uint?> ReadyPlayers = new List<uint?>();


    public GameObject networkVariablesController;
    public NetworkVariables networkVariables;
    
    
    public LobbyNetworkController lobbyController;
    [SerializeField] private Transform spawnedObjectPrefab;
    [SerializeField] private Animator playerAnimator;

    [SerializeField]
    private Camera playerCamera;

    public GameObject playerUsername;

    public Transform PlayerBody;

    public ulong playerInView = 999;


   
    private NetworkVariable<CostumData> randomNumber = new NetworkVariable<CostumData>(new CostumData
    {
        _int = 1,
        _bool = false,
    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct CostumData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes _string;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref _string);
        }
    }


     
    

    public override void OnNetworkSpawn()
    {
        /*randomNumber.OnValueChanged += (CostumData previousValue, CostumData newValue) =>
        {
            Debug.Log($"{OwnerClientId} {newValue._int}");
        };

        readyPlayers.OnValueChanged += (int previousValue, int newValue) =>
        {
            if (networkVariables == null)
            {
                networkVariables = GameObject.Find("NetworkVariables").GetComponent<NetworkVariables>();
            }
            ChangeReadyCountText(newValue); 
            Debug.Log("Changed timer");
        };*/
            
        
        
        
        
            
        
        


        if (IsOwner)
        {
            playerCamera.gameObject.SetActive(true);
            playerUsername.SetActive(false);
        }

        
    }

    private void Start()
    {
        
    }

    
    
    public void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;

        if (other.tag == "Player" && playerInView == 999)
            playerInView = other.transform.gameObject.GetComponentInParent<NetworkObject>().OwnerClientId;
        
        if (other.tag == "InteractableObject")
        {
            objectInView = true;
            // other.GetComponent<InteractableObject>().Interact();
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (!IsOwner) return;
        
        objectInView = false;
        playerInView = 999;
    }

    private void ChangeReadyCountText(int value)
    {
        if (!IsOwner) return;
        
        if (networkVariables == null)
            return;

    }

    private void Update()
    {
        if(networkVariablesController == null)
            networkVariablesController  = GameObject.Find("NetworkVariables");

        if (SceneManager.GetActiveScene().name == "Lobby" && networkVariablesController != null && networkVariables == null)
        {
            networkVariables = networkVariablesController.GetComponent<NetworkVariables>();
        }
        //ChangeReadyCountText(readyPlayers.Value);

        if (!IsOwner)
            return;
        
        this.GetComponent<BoxCollider>().center = PlayerBody.transform.position + new Vector3(0f, .3f, 0f);
        
        
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(objectInView)
                ReadyUpServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryToKillPlayerServerRpc(int clientId);
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            var spawnedObject = Instantiate(spawnedObjectPrefab);
            spawnedObject.GetComponent<NetworkObject>().Spawn(true);
            
            //TestServerRpc();
            //TestClientRpc(new ClientRpcParams{ Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>{1}}});
            randomNumber.Value = new CostumData
            {
                _int = Random.Range(10, 50),
            };
        }

       

    }

    [ServerRpc] 
    public void ReadyUpServerRpc()
    {
        if (ReadyPlayers.FirstOrDefault(x => x == OwnerClientId) == null)
        {
            ReadyPlayers.Add((uint)OwnerClientId);
            networkVariables.readyPlayers.Value += 1;
            Debug.Log($"{OwnerClientId} has ready up!");
        }
        else
        {
            ReadyPlayers.Remove((uint)OwnerClientId);
            networkVariables.readyPlayers.Value -= 1;
            Debug.Log($"{OwnerClientId} has unready!");
        }
    }

    [ServerRpc]
    private void TryToKillPlayerServerRpc(ulong clientId)
    {
        var playerNetwork = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId).GetComponent<PlayerNetwork>();
        if (playerNetwork.playerInView == 999)
        {
            Debug.Log("No player in range.");
            return;
        }
        bnkghb
        
        
    }

    [ServerRpc]
    private void TestServerRpc()
    {
        Debug.Log("COCK" + OwnerClientId);
    }

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientParams)
    {
        Debug.Log("ClientRPC " + OwnerClientId);
    }
}
