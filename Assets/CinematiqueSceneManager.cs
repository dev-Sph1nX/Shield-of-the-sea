using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CinematiqueSceneManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] string nextSceneName = "1-Beach";
    [Header("Scene Transition")]
    [SerializeField] private Animator sceneAnimator = null;
    [SerializeField] private Animator travelingAnimator = null;
    [SerializeField] public GameObject cinematiqueGameObject;
    [SerializeField] private AudioSource[] audios;
    [SerializeField] private bool stayHere = false;

    private IDialogManager cinematiqueDialogManager;


    private Image mask = null;

    void Awake()
    {
        foreach (AudioSource audio in audios)
        {
            audio.Play();
        }
        cinematiqueDialogManager = cinematiqueGameObject.GetComponent<IDialogManager>();


        sceneAnimator.gameObject.SetActive(true);
        foreach (Transform child in sceneAnimator.gameObject.transform)
        {
            if (child.gameObject.tag == "TransitionImage")
                mask = child.gameObject.GetComponent<Image>();
        }
        sceneAnimator.SetTrigger("Enter");
        Invoke("HideMask", 1f);
        Invoke("StartTraveling", 1f);
        cinematiqueDialogManager.StartDialog();
    }

    private void HideMask()
    {
        mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 0f);
    }
    private void StartTraveling()
    {
        travelingAnimator.SetTrigger("Traveling");
    }
    public void NextScene()
    {
        foreach (AudioSource audio in audios)
        {
            audio.DOFade(0, 2);
        }
        if (!stayHere)
            GameManager.Instance.ChangeScene(nextSceneName, StartAnimation);
    }

    public void StartAnimation()
    {
        sceneAnimator.SetTrigger("Exit");
    }

}
