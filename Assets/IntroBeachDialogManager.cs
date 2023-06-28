using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBeachDialogManager : MonoBehaviour, IDialogManager
{
    [Header("References")]
    [SerializeField] DialogContentManager marco;
    [SerializeField] CinematiqueSceneManager cinematiqueSceneManager;
    [SerializeField] float timeAfterText = 4f;


    List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    int index = 0;
    void Awake()
    {
        tutorialSteps.Add(new TutorialStep("Vous avez vu ?! Je vous raconte pas des salades hein !", Narrator.Marco));
        tutorialSteps.Add(new TutorialStep("C’est un vrai danger, toutes les paulettes mangent des déchets, il faut intervenir ! Battons-nous !", Narrator.Marco));
    }

    public void StartDialog()
    {
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
        StartCoroutine(InnerOnNextStep());
    }

    IEnumerator InnerOnNextStep()
    {
        yield return new WaitForSeconds(timeAfterText);
        index++;
        if (index < tutorialSteps.Count) DiscoursNextStep(tutorialSteps[index]);
        else DiscoursEnd();
    }


    public void DiscoursEnd()
    {
        marco.CloseDialog();
        Invoke("NextScene", 1f);
    }
    void NextScene()
    {

        cinematiqueSceneManager.NextScene();
    }
}

public interface IDialogManager
{
    public void StartDialog();
    public void OnNextStep();
}
