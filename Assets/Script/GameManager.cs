using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

static class Wincondition
{
    public static int[,] winConidtion = new int[,] { 
        {0,1,2}, // 1st row ->                     |            |
        {3,4,5}, // 2st row ->                 0   |     1      |    2
        {6,7,8}, // 3st row ->              -------+------------+--------
        {0,3,6}, // 1st colom ^                    |            |
        {1,4,7}, // 2st colom ^               3    |     4      |    5
        {3,5,8}, // 3st colom ^                    |            |
        {0,4,8}, // Diagonal right-down     -------+------------+--------
        {2,4,6}, // Diagonal left-down        6    |      7     |    8
    };           //                                |            |
}

public class GameManager : MonoBehaviour
{
    public bool IsPlayerturn;
    public List<Button> buttons;
    public List<TMP_Text> ButtonText;
    private NetworkClient Network;
    private int[,] WinConditions = Wincondition.winConidtion;

    // Start is called before the first frame update
    void Start()
    {
        InitiText();
        Network = FindObjectOfType<NetworkClient>();
    }
    private void Update()
    {
        UpdateButtons();
        CheckWinCondition();
    }
    public void UpdateButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (ButtonText[i].text == " ")
            {
                buttons[i].interactable = IsPlayerturn;
            }
            else if(ButtonText[i].text != " ")
            {
                buttons[i].interactable = false;
            }
        }
    }

    public string GameCurrentState()
    {
        string state = "";
        for (int i = 0; i < buttons.Count; i++)
        {
            switch (ButtonText[i].text)
            {
                case " ":
                    state += "N;";
                    break;
                case "X":
                    state += "X;";
                    break;
                case "O":
                    state += "O;";
                    break;
                default:
                    break;
            }
        }            
        return state;
    }

    public void UpdateGame(string board)
    {
        string[] Tile = board.Split(';');
        if (board != "")
        {
            for (int i = 0; i < ButtonText.Count; i++)
            {
                switch (Tile[i])
                {
                    case "X":
                        ButtonText[i].text = "X";
                        break;
                    case "O":
                        ButtonText[i].text = "O";
                        break;
                    case "N":
                        ButtonText[i].text = " ";
                        break;
                    default:
                        break;
                }
            } 
        }
    }

    private void InitiText()
    {
        foreach (Button button in buttons)
        {
            ButtonText.Add(button.GetComponentInChildren<TMP_Text>());
        }
    }

    public void CheckWinCondition()
    {
        for (int condition = 0; condition < WinConditions.GetLength(0); condition++)
        {
            if (ButtonText[WinConditions[condition, 0]].text == Network.XOrOString &&
                ButtonText[WinConditions[condition, 1]].text == Network.XOrOString &&
                ButtonText[WinConditions[condition, 2]].text == Network.XOrOString)
            {
                Network.SendWin();
                ResetBoard();
            }
        }
    }

    public void ResetBoard()
    {
        foreach (TMP_Text text in ButtonText)
        {
            text.text = " ";
        }
    }
}
