using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinPneuManager : MonoBehaviour
{
    [SerializeField] public int interactRange = 2;
    [SerializeField] PinApparition pinPlayer1;
    [SerializeField] PinApparition pinPlayer2;
    GameObject player1;
    GameObject player2;
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindWithTag("Player1");
        player2 = GameObject.FindWithTag("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer1InRangeOfAction())
            pinPlayer1.Appear();
        else
            pinPlayer1.Disappear();

        if (isPlayer2InRangeOfAction())
            pinPlayer2.Appear();
        else
            pinPlayer2.Disappear();
    }

    bool isPlayer1InRangeOfAction()
    {
        if (NPCInteractable.CheckRange(transform.position.x, player1.transform.position.x - interactRange, player1.transform.position.x + interactRange) && NPCInteractable.CheckRange(transform.position.z, player1.transform.position.z - interactRange, player1.transform.position.z + interactRange))
        {
            return true;
        }
        return false;
    }

    bool isPlayer2InRangeOfAction()
    {
        if (NPCInteractable.CheckRange(transform.position.x, player2.transform.position.x - interactRange, player2.transform.position.x + interactRange) && NPCInteractable.CheckRange(transform.position.z, player2.transform.position.z - interactRange, player2.transform.position.z + interactRange))
        {
            return true;
        }
        return false;
    }
    public void DestroyPin()
    {
        pinPlayer1.DestroyPin();
        pinPlayer2.DestroyPin();
    }
}
