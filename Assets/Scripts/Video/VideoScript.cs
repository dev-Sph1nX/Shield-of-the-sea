using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    [SerializeField] public VideoPlayer videoPlayer;
    [SerializeField] public VideoSceneManager sceneManager;
    [SerializeField] public Animator sceneAnimator;

    private void Start()
    {
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare();

        videoPlayer.loopPointReached += VideoEnded;
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        sceneAnimator.SetTrigger("Enter");
        Invoke("PlayVideo", 1f);
    }
    private void PlayVideo()
    {
        videoPlayer.Play();
    }

    void VideoEnded(VideoPlayer vp)
    {
        sceneManager.NextScene();
    }

}
