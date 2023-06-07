using WebSocketSharp;
using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

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
    void Start()
    {
        if (!GameManager.Instance.debugMode)
        {

            Debug.Log("Start connection to the server.");
            ws = new WebSocket("ws://192.168.43.109:3000");
            // ws = new WebSocket("ws://localhost:3000");

            ws.OnMessage += OnMessage;

            ws.Connect();

            ws.Send(jsonString);
        }
        else
        {
            Debug.Log("Debug Mode activate.");
        }

    }

    void Update()
    {

        if (InputSystem.Player1Interaction())
        {
            player1.gameObject.GetComponent<PlayerInteraction>().OnUserInteract();
        }
        if (InputSystem.Player2Interaction())
        {
            player2.gameObject.GetComponent<PlayerInteraction>().OnUserInteract();
        }

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
            onUserInteractionData(e);
        }
        else
        {
            onUserPositionData(e);
        }
    }

    private void onUserPositionData(MessageEventArgs e)
    {
        ServerPositionMessage serverMessage = JsonUtility.FromJson<ServerPositionMessage>(e.Data);
        if (serverMessage.data.p1 != null && serverMessage.data.p1.x != 0 && serverMessage.data.p1.y != 0)
        {
            player1.sendData(serverMessage.data.p1);
        }
        if (serverMessage.data.p2 != null && serverMessage.data.p2.x != 0 && serverMessage.data.p2.y != 0)
        {
            player2.sendData(serverMessage.data.p2);
        }
    }

    private void onUserInteractionData(MessageEventArgs e)
    {
        ServerInteractionMessage serverMessage = JsonUtility.FromJson<ServerInteractionMessage>(e.Data);
        if (serverMessage.data.player == "p1")
        {
            Debug.Log("Player 1 Interact");
            player1.gameObject.GetComponent<PlayerInteraction>().OnUserInteract();
        }
        if (serverMessage.data.player == "p2")
        {
            Debug.Log("Player 2 Interact");
            player2.gameObject.GetComponent<PlayerInteraction>().OnUserInteract();
        }
    }



    private void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }


}
