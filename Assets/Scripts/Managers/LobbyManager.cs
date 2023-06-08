using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] string animationName = "Wave";
    [SerializeField] string nextSceneName = "1-Beach";
    [Header("Scene Transition")]
    [SerializeField] private Animator sceneAnimator = null;
    [SerializeField] private string triggerName;
    [Header("References")]
    [SerializeField] Animator p1Animator = null;
    [SerializeField] Animator p2Animator = null;
    bool P1IsReady = false;
    bool P2IsReady = false;

    bool P1trigger = false;
    bool P2trigger = false;

    // Update is called once per frame
    void Update()
    {
        // if (GameManager.Instance.debugMode)
        // {
        //     P1IsReady = true;
        //     P2IsReady = true;
        // }

        if (GameManager.Instance.debugMode && InputSystem.Player1Interaction())
        {
            P1trigger = true;
        }

        if (GameManager.Instance.debugMode && InputSystem.Player2Interaction())
        {
            P2trigger = true;
        }

        if (P1trigger || InputSystem.Player1Interaction())
        {
            P1trigger = false;
            P1IsReady = !P1IsReady;
            if (P1IsReady)
            {
                p1Animator.SetTrigger(animationName);
            }
        }

        if (P2trigger || InputSystem.Player1Interaction())
        {
            P2trigger = false;
            P2IsReady = !P2IsReady;
            if (P2IsReady)
            {
                p2Animator.SetTrigger(animationName);
            }
        }

        if (P1IsReady && P2IsReady)
        {
            GameManager.Instance.ChangeScene(nextSceneName, StartAnimation);
        }
    }

    public void StartAnimation()
    {
        sceneAnimator.SetTrigger(triggerName);
    }

    public void OnPlayer1Interaction()
    {
        P1trigger = true;
    }

    public void OnPlayer2Interaction()
    {
        P2trigger = true;
    }
}
