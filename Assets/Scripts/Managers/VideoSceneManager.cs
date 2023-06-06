using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSceneManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] string nextSceneName = "1-Beach";
    [Header("Scene Transition")]
    [SerializeField] private Animator sceneAnimator = null;
    [SerializeField] private string triggerName;
    private bool isTransitioningOut = false;




    // Update is called once per frame
    void Update()
    {
        if (InputSystem.Player1Interaction() && !isTransitioningOut) // GameManager.Instance.debugMode && 
        {
            NextScene();
            isTransitioningOut = true;
        }
    }

    public void NextScene()
    {
        GameManager.Instance.ChangeScene(nextSceneName, StartAnimation);
    }

    public void StartAnimation()
    {
        sceneAnimator.SetTrigger(triggerName);
    }
}
