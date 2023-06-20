using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] string nextSceneName = "1-Beach";
    [Header("Scene Transition")]
    [SerializeField] private Animator sceneAnimator = null;
    [SerializeField] private string triggerName;
    [Header("References")]
    [SerializeField] DialogManager dialogManager;

    // Update is called once per frame
    void Update()
    {
        if (dialogManager.hasFinish)
        {
            GameManager.Instance.ChangeScene(nextSceneName, StartAnimation);
        }
    }

    public void StartAnimation()
    {
        sceneAnimator.SetTrigger(triggerName);
    }
}
