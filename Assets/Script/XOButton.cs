using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XOButton : MonoBehaviour
{
    public NetworkClient networkClient;
    public TMP_Text text;
    public GameManager gameManager;
    private void Start()
    {
        networkClient = FindObjectOfType<NetworkClient>();
        text = GetComponentInChildren<TMP_Text>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void XOButtonClick()
    {
        text.text = networkClient.XOrOString;
        gameManager.IsPlayerturn = false;
        gameManager.UpdateButtons();
        networkClient.PlayTile();
    }
}
