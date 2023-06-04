using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{


    // -----  Player 1
    static readonly string P1Interact = "P1-Interact";

    // -----  Player 2
    static readonly string P2Interact = "P2-Interact";
    // -----  Debug
    static readonly string DP1Horizontal = "DP1 - Horizontal";
    static readonly string DP2Horizontal = "DP2 - Horizontal";

    static readonly string DP1Vertical = "DP1 - Vertical";
    static readonly string DP2Vertical = "DP2 - Vertical";

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

    //  ----- Debug

    public static float getHorizontalAxis(SystemId id)
    {
        return Input.GetAxis(id == SystemId.Player1 ? DP1Horizontal : DP2Horizontal);
    }
    public static float getVerticalAxis(SystemId id)
    {
        return Input.GetAxis(id == SystemId.Player1 ? DP1Vertical : DP2Vertical);
    }

}
