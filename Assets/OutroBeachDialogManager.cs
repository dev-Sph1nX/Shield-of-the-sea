using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroBeachDialogManager : MonoBehaviour, IDialogManager
{
    [Header("References")]
    [SerializeField] DialogContentManager marco;
    [SerializeField] CinematiqueSceneManager cinematiqueSceneManager;


    List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    int index = 0;
    void Awake()
    {
        tutorialSteps.Add(new TutorialStep("Bon c’était pas si mal mais vous avez vu, des problématiques on en a des tas !", Narrator.Marco));
        tutorialSteps.Add(new TutorialStep("Notre salon est envahis de plastiques des pneus et des hameçon que vous n’avez pas réussis à attraper.", Narrator.Marco));
        tutorialSteps.Add(new TutorialStep("Notre sous sol est envahis de déchets et trop souvent remuer par les raclement de fond marin.", Narrator.Marco));
        tutorialSteps.Add(new TutorialStep("Renseigner vous, on vous a fait plein de belle affiches avec toutes les infos !", Narrator.Marco));
        tutorialSteps.Add(new TutorialStep("Aider nous à lutter !", Narrator.Marco));
    }

    public void StartDialog()
    {
        Debug.Log(tutorialSteps.Count);
        DiscoursNextStep(tutorialSteps[index]);
    }

    public void DiscoursNextStep(TutorialStep step)
    {
        setText(step);
        if (step.invokeAction != null)
        {
            step.invokeAction();
        }
    }
    private void setText(TutorialStep step)
    {
        StartCoroutine(SetInnerText(step));
    }
    private IEnumerator SetInnerText(TutorialStep step)
    {
        yield return new WaitForSeconds(1f);
        marco.SetText(step);
    }

    public void OnNextStep()
    {
        Debug.Log("OnNextStep index " + index);
        index++;
        if (index < tutorialSteps.Count) DiscoursNextStep(tutorialSteps[index]);
        else DiscoursEnd();
    }

    public void DiscoursEnd()
    {
        Debug.Log("end of discours");
        marco.CloseDialog();
        Invoke("NextScene", 1f);
    }
    void NextScene()
    {

        cinematiqueSceneManager.NextScene();
    }
}
