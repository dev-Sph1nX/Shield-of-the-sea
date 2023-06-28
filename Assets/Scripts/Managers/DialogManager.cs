using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;
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
    [SerializeField] float timeAfterText = 4f;

    [Header("References")]
    [SerializeField] DialogContentManager marco;
    [SerializeField] DialogContentManager polo;
    [SerializeField] BigModalPolo bigModalPolo;
    [SerializeField] PlayerInteraction player1;
    [SerializeField] PlayerInteraction player2;
    [SerializeField] TutoLearnWasteInteraction learnWasteInteractionManager;
    [SerializeField] WS websocket;
    [SerializeField] CanvasGroup swordMotionCanvas;
    [SerializeField] PlayerNameApparition player1NameApparition;
    [SerializeField] PlayerNameApparition player2NameApparition;
    [SerializeField] AudioSource tutorialMusic;

    List<TutorialStep> tutorialSteps = new List<TutorialStep>();

    private int index = 0;
    private bool waitStart = true, started = false, called = false;
    public bool hasFinish = false, firstRetryOnFirstCheck = true, firstRetryOnSecondCheck = true;
    // Start is called before the first frame update
    void Start()
    {
        tutorialSteps.Add(new TutorialStep("Hello nous, c'est Marco & Polo. On habite l'océan, cependant depuis peu, on entretient une relation de voisinage toxique avec les humains qui nous polluent constamment.", Narrator.Marco, pForcedTimed: true));
        tutorialSteps.Add(new TutorialStep("Aidez-nous à nous battre pour retrouver la paix et la sérénité tant attendue.", Narrator.Marco, pForcedTimed: true));
        tutorialSteps.Add(new TutorialStep("", Narrator.Any, bigModalPolo.Show));
        tutorialSteps.Add(new TutorialStep("", Narrator.Any, learnWasteInteractionManager.throwTwoWaste)); // checkpoint - 3
        tutorialSteps.Add(new TutorialStep("Tu vois ces déchets . Il faut que tu t'approches d'eux.", Narrator.Polo, learnWasteInteractionManager.waitingProximity));
        tutorialSteps.Add(new TutorialStep("Bien joué !", Narrator.Polo, pForcedTimed: true));
        tutorialSteps.Add(new TutorialStep("Vous pouvez maintenant voir un petit indicateur au-dessus des déchets. Ça veut dire que vous pouvez les détruire ! Comment ? En donnant un coup d'épée comme montré à l'écran !", Narrator.Polo, showSwordMotion, pForcedTimed: true));
        tutorialSteps.Add(new TutorialStep("Allez-y, essayez de les détruire.", Narrator.Polo, WaitingInteraction));
        tutorialSteps.Add(new TutorialStep("Super ! Une nouvelle vague arrive ! Préparez-vous !", Narrator.Polo, hideSwordMotion, pForcedTimed: true));// checkpoint - 8
        tutorialSteps.Add(new TutorialStep("", Narrator.Any, learnWasteInteractionManager.throwSecondWave));
        tutorialSteps.Add(new TutorialStep("Trop fort ! Maintenant que vous êtes prêt, vous allez pouvoir rentrer dans les choses sérieuses. Que le vent vous soit favorable et bonne expérience !", Narrator.Polo, pForcedTimed: true));
        if (GameManager.Instance.passTutorial) // GameManager.Instance.debugMode &&
            hasFinish = true;
        else
            UpdatePlayerInteraction(false);
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

                    player1NameApparition.Appear();
                    player2NameApparition.Appear();

                    Debug.Log("cbziab");

                    tutorialMusic.Play();
                    tutorialMusic.DOFade(tutorialMusic.volume, 4);
                }
                else
                {
                    if (!called)
                    {
                        called = true;
                        Invoke("StartDialog", 5f);
                        player1NameApparition.Appear();
                        player2NameApparition.Appear();

                        tutorialMusic.Play();
                        tutorialMusic.DOFade(tutorialMusic.volume, 4);
                    }
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
        StartCoroutine(InnerOnNextStep());
    }
    IEnumerator InnerOnNextStep()
    {

        yield return new WaitForSeconds(tutorialSteps[index].forcedTimed ? timeAfterText : 0f);

        index++;
        if (index < tutorialSteps.Count) TutorialNextStep(tutorialSteps[index]);
        else TutorialEnd();
    }

    public void RestartStep()
    {
        ResetEntities();
        if (index >= 3 && index <= 8)
        {
            index = 3;
            if (firstRetryOnFirstCheck)
            {
                firstRetryOnFirstCheck = false;
                tutorialSteps.Insert(index, new TutorialStep("Tu sais, ça arrive à tout le monde de rater, ce n'est pas la fin du monde. Allez réessayer pour voir ! ", pForcedTimed: true));
            }
        }
        else if (index > 8)
        {
            index = firstRetryOnFirstCheck ? 9 : 10;
            if (firstRetryOnSecondCheck)
            {
                firstRetryOnSecondCheck = false;
                tutorialSteps.Insert(index, new TutorialStep("Tu sais, ça arrive à tout le monde de rater, ce n'est pas la fin du monde. Allez réessayer pour voir ! ", pForcedTimed: true));
            }
            UpdatePlayerInteraction(true);
        }
        index--;
        OnNextStep();
    }

    public void ResetEntities()
    {
        swordMotionCanvas.DOFade(0, 1);

        UpdatePlayerInteraction(false);

        NPCInteractable[] wastes = FindObjectsOfType<NPCInteractable>();
        foreach (NPCInteractable w in wastes)
        {
            WasteShadow shadowManager = w.GetComponent<WasteShadow>();
            if (shadowManager)
            {
                shadowManager.DestroyShadow();
            }
            PinApparition pinApparition = w.GetComponent<PinApparition>();
            if (pinApparition)
            {
                pinApparition.DestroyPin();
            }
            PinPneuManager pinPneuManager = w.GetComponent<PinPneuManager>();
            if (pinPneuManager)
                pinPneuManager.DestroyPin();
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
        marco.CloseDialog();
        polo.CloseDialog();
        UpdatePlayerInteraction(true);
        tutorialMusic.DOFade(0, 1);
        Invoke("SetHasFinish", 1f);
    }

    void SetHasFinish()
    {

        hasFinish = true;

    }

    public void showSwordMotion()
    {
        swordMotionCanvas.DOFade(1, 1);

        UpdatePlayerInteraction(true);

        // for the indicator
        player1.SetCanInteract(false);
        player2.SetCanInteract(false);

    }

    public void WaitingInteraction()
    {
        player1.SetCanInteract(true);
        player2.SetCanInteract(true);

        learnWasteInteractionManager.waitingInteraction();
    }

    void hideSwordMotion()
    {
        swordMotionCanvas.DOFade(0, 1);
    }
}

