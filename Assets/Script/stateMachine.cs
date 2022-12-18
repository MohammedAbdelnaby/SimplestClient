using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateMachine : MonoBehaviour
{
    private static stateMachine instance;

    [SerializeField]
    private int scene;
    public static stateMachine Instance { get => instance; set => instance = value; }

    [SerializeField]
    private GameObject MainMenu;        // 0
    [SerializeField]
    private GameObject LoginScene;      // 1
    [SerializeField]
    private GameObject LobbyScene;      // 2
    [SerializeField]
    private GameObject CreateRoomScene; // 3
    [SerializeField]
    private GameObject JoinRoomScene;   // 4
    [SerializeField]
    private GameObject GameScene;       // 5
    [SerializeField]
    private GameObject WinScene;        // 6
    [SerializeField]
    private GameObject LostScene;       // 7

    public int Scene
    {
        get { return scene; }
        set
        {
            if (value < 0)
            {
                Debug.Log("SCENES CANT BE LOWER THAN 0");
            }
            else
            {
                ChangeScene(value);
                scene = value;
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
    private void ChangeScene(int pvalue)
    {
        switch (pvalue)
        {
            case 0:
                DeactivateCurrentScene();
                MainMenu.SetActive(true);
                break;
            case 1:
                DeactivateCurrentScene();
                LoginScene.SetActive(true);
                break;
            case 2:
                DeactivateCurrentScene();
                LobbyScene.SetActive(true);
                break;
            case 3:
                DeactivateCurrentScene();
                CreateRoomScene.SetActive(true);
                break;
            case 4:
                DeactivateCurrentScene();
                JoinRoomScene.SetActive(true);
                break;
            case 5:
                DeactivateCurrentScene();
                GameScene.SetActive(true);
                break;
            case 6:
                DeactivateCurrentScene();
                WinScene.SetActive(true);
                break;
            case 7:
                DeactivateCurrentScene();
                LostScene.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void DeactivateCurrentScene()
    {
        switch (Scene)
        {
            case 0:
                MainMenu.SetActive(false);
                break;
            case 1:
                LoginScene.SetActive(false);
                break;
            case 2:
                LobbyScene.SetActive(false);
                break;
            case 3:
                CreateRoomScene.SetActive(false);
                break;
            case 4:
                JoinRoomScene.SetActive(false);
                break;
            case 5:
                GameScene.SetActive(false);
                break;
            case 6:
                WinScene.SetActive(false);
                break;
            case 7:
                LostScene.SetActive(false);
                break;
            default:
                break;
        }
    }
}
