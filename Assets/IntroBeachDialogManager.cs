using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBeachDialogManager : MonoBehaviour, IDialogManager
{
    [Header("References")]
    [SerializeField] DialogContentManager marco;
    [SerializeField] CinematiqueSceneManager cinematiqueSceneManager;


    List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    int index = 0;
    void Awake()
    {
        tutorialSteps.Add(new TutorialStep("Vous avez vu ?! Je vous raconte pas des salades hein !", Narrator.Marco));
        tutorialSteps.Add(new TutorialStep("C’est un vrai danger toutes les paulettes mangent des déchets il faut intervenir battons nous !", Narrator.Marco));
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

public interface IDialogManager
{
    public void StartDialog();
    public void OnNextStep();
}
