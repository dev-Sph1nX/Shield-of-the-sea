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

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.debugMode)
        {
            P1IsReady = true;
            P2IsReady = true;
        }

        if (InputSystem.Player1Interaction())
        {
            P1IsReady = !P1IsReady;
            if (P1IsReady)
            {
                Debug.Log("P1 ready !");
                p1Animator.SetTrigger(animationName);
            }
            else
            {
                Debug.Log("P1 not ready !");
            }
        }

        if (InputSystem.Player2Interaction())
        {
            P2IsReady = !P2IsReady;
            if (P2IsReady)
            {
                Debug.Log("P2 ready !");
                p2Animator.SetTrigger(animationName);
            }
            else
            {
                Debug.Log("P2 not ready !");
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
}
