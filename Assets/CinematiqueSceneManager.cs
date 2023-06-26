using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematiqueSceneManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] string nextSceneName = "1-Beach";
    [Header("Scene Transition")]
    [SerializeField] private Animator sceneAnimator = null;
    [SerializeField] private Animator travelingAnimator = null;
    [SerializeField] public GameObject cinematiqueGameObject;
    [SerializeField] public IntroBeachDialogManager introBeachDialog;

    private IDialogManager cinematiqueDialogManager;


    private Image mask = null;

    void Awake()
    {
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
        introBeachDialog.StartDialog();
    }
    public void NextScene()
    {
        GameManager.Instance.ChangeScene(nextSceneName, StartAnimation);
    }

    public void StartAnimation()
    {
        sceneAnimator.SetTrigger("Exit");
    }

}
