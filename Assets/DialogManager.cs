using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
public class TutorialStep
{
    public string text;
    public delegate void OnInvokeAction();
    public OnInvokeAction invokeAction;

    public TutorialStep(string pText, OnInvokeAction pInvokeAction = null)
    {
        text = pText;
        invokeAction = pInvokeAction;
    }
}

public class DialogManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] TimeIndicator timeIndicator;
    [SerializeField] GameObject[] players;

    List<TutorialStep> tutorialSteps = new List<TutorialStep>();

    private int index = 0;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        tutorialSteps.Add(new TutorialStep("Bienvenue dans WAKE ! Ca va secouer !"));
        tutorialSteps.Add(new TutorialStep("Vous allez apprendre à vous deplacer mtn", LearnMovement));
        tutorialSteps.Add(new TutorialStep("Supper, apprenez a taper maintenant :)"));

        animator = GetComponent<Animator>();

        if (!GameManager.Instance.passTutorial)
        {
            UpdatePlayerInteraction(false);
            ShowDialog();
            TutorialNextStep(tutorialSteps[index]);
        }
    }

    public void TutorialNextStep(TutorialStep step)
    {
        StartCoroutine(setText(step));
        Debug.Log(step.invokeAction);
        if (step.invokeAction != null)
        {
            step.invokeAction();
        }
    }

    public void ShowDialog()
    {
        animator.SetTrigger("Open");
    }

    public void CloseDialog()
    {
        animator.SetTrigger("Close");
    }

    private void UpdatePlayerInteraction(bool enabled)
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerInteraction>().enabled = enabled;
        }
    }

    private IEnumerator setText(TutorialStep step)
    {
        yield return new WaitForSeconds(2f);
        timeIndicator.StartTimer(step.invokeAction == null);
        dialogText.text = step.text;
    }

    public void OnNextStep()
    {
        Debug.Log("OnNextStep ");
        index++;
        if (index < tutorialSteps.Count) TutorialNextStep(tutorialSteps[index]);
        else TutorialEnd();
    }

    void LearnMovement()
    {
        Debug.Log("LearnMovement, in 5s");
        Invoke("onActionDone", 5f);
    }

    public void onActionDone()
    {
        OnNextStep();
    }

    public void TutorialEnd()
    {
        Debug.Log("end of tutoriel !!!!!!");
        CloseDialog();
        UpdatePlayerInteraction(true);
    }
}

