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
    [SerializeField] private AudioSource[] audios;

    private bool isTransitioningOut = false;

    void Start()
    {
        foreach (AudioSource audio in audios)
        {
            audio.Play();
        }
    }

    void Update()
    {
        if (!isTransitioningOut && InputSystem.Player1Interaction())
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
