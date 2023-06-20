using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using System;

public enum Narrator : int
{
    Marco = 0,
    Polo = 1,
    Any = 2,
}
public class TutorialStep
{
    public string text;
    public bool forcedTimed;
    public delegate void OnInvokeAction();
    public OnInvokeAction invokeAction;
    public Narrator narrator;

    public TutorialStep(string pText, Narrator pNarrator = Narrator.Polo, OnInvokeAction pInvokeAction = null, bool pForcedTimed = false)
    {
        text = pText;
        narrator = pNarrator;
        invokeAction = pInvokeAction;
        forcedTimed = pForcedTimed;
    }

    override public string ToString()
    {
        return "text :" + text;
    }
}

public class DialogManager : MonoBehaviour, IDialogManager
{
    [Header("References")]
    [SerializeField] DialogContentManager marco;
    [SerializeField] DialogContentManager polo;
    [SerializeField] BigModalPolo bigModalPolo;
    [SerializeField] PlayerInteraction player1;
    [SerializeField] PlayerInteraction player2;
    [SerializeField] TutoLearnMovement learnMovementManager;
    [SerializeField] TutoLearnInteraction learnInteractionManager;
    [SerializeField] TutoLearnWasteInteraction learnWasteInteractionManager;
    [SerializeField] WS websocket;
    [SerializeField] Animator swordMotionAnimator;
    [SerializeField] VideoPlayer swordMotionVideoPlayer;

    List<TutorialStep> tutorialSteps = new List<TutorialStep>();

    private int index = 0;
    private bool waitStart = true, started = false;
    public bool hasFinish = false, firstRetryOnFirstCheck = true, firstRetryOnSecondCheck = true;
    // Start is called before the first frame update
    void Start()
    {
        tutorialSteps.Add(new TutorialStep("Hello nous c’est Marco & Polo. On habite l’océan, cependant depuis peu on entretient une relation de voisinage toxique avec les humains qui nous polluent constamment.", Narrator.Marco));
        tutorialSteps.Add(new TutorialStep("Aidez-nous à nous battre pour retrouver la paix et la sérénité tant attendu.", Narrator.Marco));
        tutorialSteps.Add(new TutorialStep("", Narrator.Any, bigModalPolo.Show));
        tutorialSteps.Add(new TutorialStep("", Narrator.Any, learnWasteInteractionManager.throwTwoWaste)); // checkpoint - 3
        tutorialSteps.Add(new TutorialStep("Tu vois ces déchets ? Il faut que tu t'approches d'eux.", Narrator.Polo, learnWasteInteractionManager.waitingProximity));
        tutorialSteps.Add(new TutorialStep("Bien joué !", Narrator.Polo));
        tutorialSteps.Add(new TutorialStep("Maintenant que vous êtes à coté d'eux, vous pouvons voir qu'ils sont en surbrillance. Ca veut que vous pouvez les détruire ! Comment ? En donnant un coup d'épée comme montré à l'écran !", Narrator.Polo, showSwordMotion, true));
        tutorialSteps.Add(new TutorialStep("Allez-y, essaye de les détruire.", Narrator.Polo, WaitingInteraction));
        tutorialSteps.Add(new TutorialStep("Une nouvelle vague arrive ! Préparez-vous !", Narrator.Polo));// checkpoint - 8
        tutorialSteps.Add(new TutorialStep("", Narrator.Any, learnWasteInteractionManager.throwSecondWave));
        tutorialSteps.Add(new TutorialStep("Trop fort ! Maintenant que vous êtes prêt, vous allez pouvoir rentrez dans les choses sérieuses. Que le vent vous soit favorable et bonne expérience !", Narrator.Polo));
        if (!GameManager.Instance.passTutorial)
        {
            UpdatePlayerInteraction(false);
        }
        else
            hasFinish = true;
    }
    void Update()
    {
        if (waitStart)
        {
            if (websocket.firstSend || (GameManager.Instance.debugMode && !GameManager.Instance.passTutorial && InputSystem.Player1Interaction()))
            {
                if (GameManager.Instance.debugMode)
                {
                    StartDialog();
                }
                else
                {
                    Debug.Log("Wait 5s to start");
                    Invoke("StartTuto", 5f);
                }
            }
        }
        else
        {
            if (!started)
            {
                started = true;
                TutorialNextStep(tutorialSteps[index]);
            }
        }
    }
    public void StartDialog()
    {
        Debug.Log("Start tuto");
        waitStart = false;
    }

    public void TutorialNextStep(TutorialStep step)
    {
        setText(step);
        if (step.invokeAction != null)
        {
            step.invokeAction();
        }
    }

    private void UpdatePlayerInteraction(bool enabled)
    {
        Debug.Log("Update interaction on " + enabled);
        player1.enabled = enabled;
        player2.enabled = enabled;
    }

    private void setText(TutorialStep step)
    {
        if (step.narrator == Narrator.Marco)
        {
            polo.CloseDialog();
            StartCoroutine(SetInnerText(step, marco));
        }
        else if (step.narrator == Narrator.Polo)
        {
            marco.CloseDialog();
            StartCoroutine(SetInnerText(step, polo));
        }
        else
        {
            polo.CloseDialog();
            marco.CloseDialog();
        }
    }
    private IEnumerator SetInnerText(TutorialStep step, DialogContentManager dialogContentManager)
    {
        yield return new WaitForSeconds(1f);
        dialogContentManager.SetText(step);
    }

    public void OnNextStep()
    {
        Debug.Log("OnNextStep index " + index);
        index++;
        if (index < tutorialSteps.Count) TutorialNextStep(tutorialSteps[index]);
        else TutorialEnd();
    }

    public void RestartStep()
    {
        Debug.Log("RestartStep index " + index);
        ResetEntities();
        if (index >= 3 && index <= 8)
        {
            index = 3;
            if (firstRetryOnFirstCheck)
            {
                firstRetryOnFirstCheck = false;
                tutorialSteps.Insert(index, new TutorialStep("Tu sais, ça arrive à tout le monde de rater, c'est pas la fin du monde. Allez réessaye pour voir ! "));
            }
        }
        else if (index > 8)
        {
            index = firstRetryOnFirstCheck ? 8 : 9;
            if (firstRetryOnSecondCheck)
            {
                firstRetryOnSecondCheck = false;
                tutorialSteps.Insert(index, new TutorialStep("Tu sais, ça arrive à tout le monde de rater, c'est pas la fin du monde. Allez réessaye pour voir ! "));
            }
        }
        Debug.Log("insert at index " + index);
        index--;
        OnNextStep();
        int inner = 0;
        foreach (TutorialStep step in tutorialSteps)
        {
            Debug.Log(inner + " - " + step.ToString());
            inner++;
        }
    }

    public void ResetEntities()
    {
        swordMotionAnimator.Rebind();
        swordMotionAnimator.Update(0f);
        swordMotionVideoPlayer.frame = 0;

        UpdatePlayerInteraction(false);

        NPCInteractable[] wastes = FindObjectsOfType<NPCInteractable>();
        foreach (NPCInteractable w in wastes)
        {
            WasteShadow shadowManager = w.GetComponent<WasteShadow>();
            if (shadowManager)
            {
                shadowManager.DestroyShadow();
            }
            Destroy(w.gameObject);
        }

        TutorialTarget[] targets = FindObjectsOfType<TutorialTarget>();
        foreach (TutorialTarget target in targets)
        {
            Destroy(target.gameObject);
        }
    }

    public void TutorialEnd()
    {
        Debug.Log("end of tutoriel !!!!!!");
        marco.CloseDialog();
        polo.CloseDialog();
        hasFinish = true;
        UpdatePlayerInteraction(true);
    }

    public void showSwordMotion()
    {
        swordMotionAnimator.SetTrigger("Show");
        swordMotionVideoPlayer.Play();
        Invoke("hideSwordMotion", 5f); // SHIT -- have to be the same of time indicator
    }

    void hideSwordMotion()
    {
        swordMotionVideoPlayer.Stop();
        swordMotionAnimator.SetTrigger("Hide");
    }

    public void WaitingInteraction()
    {
        UpdatePlayerInteraction(true);
        learnWasteInteractionManager.waitingInteraction();
    }

    // public void StartLearningInteractionWithP1()
    // {
    //     player1.enabled = true;
    //     player2.enabled = false;
    //     learnInteractionManager.InteractionWithP1();
    // }
    // public void StartLearningInteractionWithP2()
    // {
    //     player1.enabled = false;
    //     player2.enabled = true;
    //     learnInteractionManager.InteractionWithP2();
    // }

    // public void throwForP1()
    // {
    //     player1.enabled = true;
    //     player2.enabled = false;
    //     learnWasteInteractionManager.throwForP1();
    // }
    // public void throwForP2()
    // {
    //     player1.enabled = false;
    //     player2.enabled = true;
    //     learnWasteInteractionManager.throwForP2();
    // }

    // public void throwForBoth()
    // {
    //     player1.enabled = true;
    //     player2.enabled = true;
    //     learnWasteInteractionManager.throwForBoth();
    // }

}

