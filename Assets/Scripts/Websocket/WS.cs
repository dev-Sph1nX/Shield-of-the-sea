using WebSocketSharp;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System;
using TMPro;

[System.Serializable]
public class ServerPositionMessage
{
    [SerializeField] public string type;

    [SerializeField] public Coords data;
}

#region Nullable annotations

#nullable enable

[System.Serializable]
public class Coords
{
    [SerializeField] public Coord? p1;
    [SerializeField] public Coord? p2;
    [SerializeField] public Coord? e1;
    [SerializeField] public Coord? e2;
}
#nullable disable

#endregion
[System.Serializable]
public class Coord
{
    [SerializeField] public float x;

    [SerializeField] public float y;
}

[System.Serializable]
public class ServerInteractionMessage
{
    [SerializeField] public string type;

    [SerializeField] public Interaction data;
}



[System.Serializable]
public class Interaction
{
    [SerializeField] public string player;
}


public class WS : MonoBehaviour
{
    WebSocket ws;

    string jsonString = "{\"type\":\"INIT\",\"data\":{\"name\":\"unity\"}}";
    [SerializeField] public PlayerMovement player1;
    [SerializeField] public PlayerMovement player2;
    [SerializeField] public TextMeshProUGUI stateText;
    private PlayerInteraction player1Interaction;
    private PlayerInteraction player2Interaction;
    private string sceneName;
    public bool firstSend = false;

    void Start()
    {
        if (!GameManager.Instance.debugMode)
        {

            Debug.Log("Connected to the server.");
            try
            {
                // ws = new WebSocket("ws://192.168.43.109:3000");
                ws = new WebSocket("ws://localhost:3000");
                stateText.text = "Connected";

                // ws = new WebSocket("ws://localhost:3000");

                ws.OnMessage += OnMessage;

                ws.Connect();

                ws.Send(jsonString);
            }
            catch
            {
                stateText.text = "Not connected";
            }
        }
        else
        {
            Debug.Log("Debug Mode activate.");
        }
        UpdateInteraction();
        sceneName = SceneManager.GetActiveScene().name;

    }

    void Update()
    {

        // if (InputSystem.Player1Interaction())
        // {
        //     OnDebugMessage("{\"type\":\"ARUCO\",\"data\":{\"p1\":{\"x\":33.3,\"y\":19.8},\"p2\":\"null\",\"e1\":\"null\",\"e2\":\"null\"}}");
        //     // OnDebugMessage("{\"type\":\"BLUE\",\"data\":{\"player\":\"p1\"}}");
        //     // player1.gameObject.GetComponent<PlayerInteraction>().OnUserInteract();
        // }
        // if (InputSystem.Player2Interaction())
        // {
        //     OnDebugMessage("{\"type\":\"BLUE\",\"data\":{\"player\":\"p2\"}}");
        //     // player2.gameObject.GetComponent<PlayerInteraction>().OnUserInteract();
        // }

        if (ws == null)
        {
            return;
        }
    }

    void OnMessage(object sender, MessageEventArgs e)
    {
        ServerPositionMessage serverMessage = JsonUtility.FromJson<ServerPositionMessage>(e.Data);

        if (serverMessage.type == "BLUE")
        {
            onUserInteractionData(e.Data);
        }
        else
        {
            if (!firstSend) firstSend = true;
            onUserPositionData(serverMessage);
        }
    }

    // private void OnDebugMessage(string data)
    // {
    //     ServerPositionMessage serverMessage = JsonUtility.FromJson<ServerPositionMessage>(data);
    //     if (serverMessage.type == "BLUE")
    //     {
    //         onUserInteractionData(data);
    //     }
    //     else
    //     {
    //         onUserPositionData(serverMessage);
    //     }
    // }

    private void onUserPositionData(ServerPositionMessage serverMessage)
    {
        if (serverMessage.data.p1 != null && serverMessage.data.p1.x != 0 && serverMessage.data.p1.y != 0)
        {
            player1.sendData(serverMessage.data.p1);
        }
        if (serverMessage.data.p2 != null && serverMessage.data.p2.x != 0 && serverMessage.data.p2.y != 0)
        {
            player2.sendData(serverMessage.data.p2);
        }
    }

    private void onUserInteractionData(string data)
    {
        ServerInteractionMessage serverMessage = JsonUtility.FromJson<ServerInteractionMessage>(data);
        if (serverMessage.data.player == "p1")
        {
            player1Interaction.GetInteractionFromWS();
        }
        if (serverMessage.data.player == "p2")
        {
            player2Interaction.GetInteractionFromWS();
        }
    }

    private void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }

    public void UpdateInteraction()
    {
        player1Interaction = player1.gameObject.GetComponent<PlayerInteraction>();
        player2Interaction = player2.gameObject.GetComponent<PlayerInteraction>();
    }


}
