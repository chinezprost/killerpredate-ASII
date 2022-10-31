using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MantainTextToCamera : NetworkBehaviour
{
    public Camera mainCamera;
    public GameObject usernameText;

    public PlayerData playerData;

    public void Start()
    {
        mainCamera = Camera.main;
    }

    public void Update()
    {
        if (mainCamera == null) return;
        usernameText.GetComponent<RectTransform>().rotation = mainCamera.transform.rotation;

        if (usernameText.GetComponent<TMP_Text>().text != "undefined") return;
        usernameText.GetComponent<TMP_Text>().text = playerData.playerUsername.Value.ToString();
    }
}
