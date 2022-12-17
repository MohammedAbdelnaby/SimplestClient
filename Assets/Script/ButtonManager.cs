using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonManager : MonoBehaviour
{
    public void MainMenuButton()
    {
        stateMachine.Instance.Scene = 0;
    }
    public void StartButton()
    {
        stateMachine.Instance.Scene = 1;
    }
    public void LobbyButton()
    {
        stateMachine.Instance.Scene = 2;
    }
    public void CreateRoomButton()
    {
        stateMachine.Instance.Scene = 3;
    }
    public void JoinRoomButton()
    {
        stateMachine.Instance.Scene = 4;
    }
    public void GameButton()
    {
        stateMachine.Instance.Scene = 5;
    }


}
