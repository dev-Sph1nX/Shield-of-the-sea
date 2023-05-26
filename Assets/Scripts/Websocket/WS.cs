using WebSocketSharp;
using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

[System.Serializable]
public class ServerMessage
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

public class WS : MonoBehaviour
{
    WebSocket ws;

    string jsonString = "{\"type\":\"INIT\",\"data\":{\"name\":\"unity\"}}";
    [SerializeField] PlayerMovement player1;
    [SerializeField] PlayerMovement player2;
    [SerializeField] VariableSystem gameManager;
    void Start()
    {
        if (!gameManager.debugMode)
        {

            Debug.Log("Start connection to the server.");
            // ws = new WebSocket("ws://192.168.43.109:3000");
            ws = new WebSocket("ws://localhost:3000");

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
        if (ws == null)
        {
            return;
        }

    }

    void OnMessage(object sender, MessageEventArgs e)
    {
        ServerMessage serverMessage = JsonUtility.FromJson<ServerMessage>(e.Data);
        if (serverMessage.data.p1 != null && serverMessage.data.p1.x != 0 && serverMessage.data.p1.y != 0)
        {
            player1.sendData(serverMessage.data.p1);
        }
        if (serverMessage.data.p2 != null && serverMessage.data.p2.x != 0 && serverMessage.data.p2.y != 0)
        {
            player2.sendData(serverMessage.data.p2);
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
