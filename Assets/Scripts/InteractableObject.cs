using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject interactObject;

    
    public void Interact()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerInteractCollider")
        {
            interactObject.SetActive(true);
        }
        
    }
    
    public void OnTriggerExit(Collider other)
    {
        interactObject.SetActive(false);
    }
}
