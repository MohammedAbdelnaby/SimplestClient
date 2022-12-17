using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool IsPlayerturn;
    public List<Button> buttons;
    public List<TMP_Text> ButtonText;

    // Start is called before the first frame update
    void Start()
    {
        InitiText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = IsPlayerturn;
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
        IsPlayerturn = true;
    }

    private void InitiText()
    {
        foreach (Button button in buttons)
        {
            ButtonText.Add(button.GetComponentInChildren<TMP_Text>());
        }
    }
}
