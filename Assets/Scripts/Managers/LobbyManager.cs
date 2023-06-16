using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] TextMeshProUGUI p1Text = null;
    [SerializeField] TextMeshProUGUI p2Text = null;
    [SerializeField] DialogManager dialogManager;

    bool P1IsReady = false;
    bool P2IsReady = false;

    bool P1trigger = false;
    bool P2trigger = false;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.passTutorial)
        {
            P1IsReady = true;
            P2IsReady = true;
        }

        if (P1trigger)
        {
            P1trigger = false;
            P1IsReady = !P1IsReady;
            if (P1IsReady)
            {
                p1Text.text = "Red Player is ready !";
                p1Animator.SetTrigger(animationName);
            }
            else
            {
                p1Text.text = "";
            }
        }

        if (P2trigger)
        {
            P2trigger = false;
            P2IsReady = !P2IsReady;
            if (P2IsReady)
            {
                p2Text.text = "Blue Player is ready !";
                p2Animator.SetTrigger(animationName);
            }
            else
            {
                p2Text.text = "";
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
        if (dialogManager.hasFinish)
            P1trigger = true;
    }

    public void OnPlayer2Interaction()
    {
        if (dialogManager.hasFinish)
            P2trigger = true;
    }
}
