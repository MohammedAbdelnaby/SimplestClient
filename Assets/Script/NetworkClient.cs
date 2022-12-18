using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class NetworkClient : MonoBehaviour
{
    /*
     * TO DO LIST:
     * - Add update state to server.
     * - Add winning and losing
     * - Add Leaving will update the game
     */

    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5491;
    byte error;
    bool isConnected = false;
    int ourClientID;
    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private TMP_InputField LoginInputField;

    [SerializeField]
    private TMP_InputField PasswordInputField;

    [SerializeField]
    private TMP_InputField CreateRoomInputField;

    [SerializeField]
    private TMP_InputField JoinRoomInputField;

    [SerializeField]
    private Toggle SignUpToggle;

    [SerializeReference]
    private TMP_Text GameRoomText;

    [SerializeField]
    private string GameRoomName;

    private string XOrO;
    public GameManager gameManager;

    public string XOrOString { get => XOrO; set => XOrO = value; }

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNetworkConnection();
    }

    public void SendLogIn()
    {
        SendMessageToHost(SignUpToggle.isOn + "," + LoginInputField.text + "," + PasswordInputField.text);
        UpdateNetworkConnection();
    }

    public void CreateRoom()
    {
        GameRoomName = CreateRoomInputField.text;
        SendMessageToHost("Create," + CreateRoomInputField.text);
        UpdateNetworkConnection();
        UpdateGame();
    }

    public void JoinRoom()
    {
        SendMessageToHost("Join," + JoinRoomInputField.text);
        UpdateNetworkConnection();
    }

    public void SendWin()
    {
        stateMachine.Instance.Scene = 6;
        SendMessageToHost("Won," + GameRoomName);
        UpdateNetworkConnection();
    }

    public void LeaveRoom()
    {
        SendMessageToHost("Leave," + GameRoomName);
        UpdateNetworkConnection();
    }

    public void PlayTile()
    {
        SendMessageToHost("PlayTile,"+ GameRoomName + "," + gameManager.GameCurrentState());
        UpdateNetworkConnection();
    }

    public void UpdateGame()
    {
        SendMessageToHost("Update Game," + GameRoomName);
        UpdateNetworkConnection();
    }

    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    Debug.Log(msg);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }

    private void Connect()
    {
        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.2.58", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);

            }
        }
    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }

    public void SendMessageToHost(string msg)
    {
        Debug.Log(msg);
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg recieved = " + msg + ".  connection id = " + id);

        switch (msg)
        {
            case "Account Created, Login now":
                text.text = msg;
                LoginInputField.text = "";
                PasswordInputField.text = "";
                break;
            case "Username is already used":
                text.text = msg;
                break;
            case "Logged in!":
                text.text = msg;
                stateMachine.Instance.Scene = 2;
                break;
            case "Wrong Username":
                text.text = msg;
                break;
            case "Wrong Password":
                text.text = msg;
                break;
            case "Room Created":
                GameRoomText.text = GameRoomName + " Room";
                stateMachine.Instance.Scene = 5;
                gameManager.IsPlayerturn = false;
                XOrO = "X";
                break;
            case "Joined":
                GameRoomName = JoinRoomInputField.text;
                GameRoomText.text = GameRoomName + " Room";
                gameManager.IsPlayerturn = false;
                stateMachine.Instance.Scene = 5;
                XOrO = "O";
                break;
            case "Leave":
                stateMachine.Instance.Scene = 2;
                GameRoomName = "";
                gameManager.ResetBoard();
                XOrO = "";
                break;
            case "Is player turn":
                gameManager.IsPlayerturn = true;
                break;
            case "Player Lost":
                stateMachine.Instance.Scene = 7;
                gameManager.ResetBoard();
                break;
            case "Player Won":
                stateMachine.Instance.Scene = 6;
                gameManager.ResetBoard();
                break;
            default:
                break;
        }

        string[] msgSplit = msg.Split(',');
        switch (msgSplit[0])
        {
            case "Opponent":
                gameManager.UpdateGame(msgSplit[1]);
                break;
            default:
                break;
        }
    }

    public bool IsConnected()
    {
        return isConnected;
    }
}