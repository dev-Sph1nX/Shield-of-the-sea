using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    //  ----- Player 1
    static readonly string P1Interact = "P1-Interact";

    // -----  Player 2
    static readonly string P2Interact = "P2-Interact";

    public static bool Player1Interaction()
    {
        return Input.GetButtonDown(P1Interact);
    }
    public static bool Player2Interaction()
    {
        return Input.GetButtonDown(P2Interact);
    }
    public static bool getButton(string buttonName)
    {
        return Input.GetButtonDown(buttonName);
    }

}
