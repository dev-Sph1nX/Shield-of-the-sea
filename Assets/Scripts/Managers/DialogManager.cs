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
    [SerializeField] PlayerInteraction player1;
    [SerializeField] PlayerInteraction player2;
    [SerializeField] TutoLearnMovement learnMovementManager;
    [SerializeField] TutoLearnInteraction learnInteractionManager;
    [SerializeField] WS websocket;

    List<TutorialStep> tutorialSteps = new List<TutorialStep>();

    private int index = 0;
    private float firstDelay = 0;
    private Animator animator;
    private bool waitStart = true, started = false;
    public bool hasFinish = false;
    // Start is called before the first frame update
    void Start()
    {
        tutorialSteps.Add(new TutorialStep("Bienvenue dans WAKE ! Ca va secouer !"));
        tutorialSteps.Add(new TutorialStep("Vous allez apprendre à vous deplacer"));
        tutorialSteps.Add(new TutorialStep("chacun doit aller sur se deplacer sur la cible de votre couleur", learnMovementManager.MovetoTarget1));
        tutorialSteps.Add(new TutorialStep("supper ! vous pouvez continuer hehe", learnMovementManager.MovetoTarget2));
        tutorialSteps.Add(new TutorialStep("encore un petit tour de manége !", learnMovementManager.MovetoTarget3));
        tutorialSteps.Add(new TutorialStep("et un autre", learnMovementManager.MovetoTarget4));
        tutorialSteps.Add(new TutorialStep("allez juste un petit dernier", learnMovementManager.MovetoTarget5));
        tutorialSteps.Add(new TutorialStep("bien normalement vous savez vous deplacer maintenant"));
        tutorialSteps.Add(new TutorialStep("on va passer à comment on interagit avec le jeu"));
        tutorialSteps.Add(new TutorialStep("regarde, il suffit d'agiter ton épée comme ça (il y aura surement une vidéo à montrer)"));
        tutorialSteps.Add(new TutorialStep("vasy essayer joueur 1 (le rouge :) )", StartLearningInteractionWithP1));
        tutorialSteps.Add(new TutorialStep("bien joue ! "));
        tutorialSteps.Add(new TutorialStep("a ton tour joueur 2 (le blue :) )", StartLearningInteractionWithP2));
        tutorialSteps.Add(new TutorialStep("super ! "));
        tutorialSteps.Add(new TutorialStep("Et voila le tuto est terminé. "));
        tutorialSteps.Add(new TutorialStep("Pour commencer le jeu, il faudra vous mettre pret. Pour être pret, rien de plus simple : donner un grand coup d'épée ! "));
        tutorialSteps.Add(new TutorialStep("Que le vent vous soit favorable et bonne expérience !"));

        animator = GetComponent<Animator>();

        if (!GameManager.Instance.passTutorial)
        {
            UpdatePlayerInteraction(false);
        }
    }
    void Update()
    {
        if (waitStart)
        {
            if (websocket.firstSend || (GameManager.Instance.debugMode && InputSystem.Player1Interaction()))
            {
                Debug.Log("websocket.firstSend => Start tuto");
                waitStart = false;
                ShowDialog();
                firstDelay = Time.time;
            }
        }
        else
        {
            if (Time.time > firstDelay + 2 && !started)
            {
                started = true;
                Debug.Log("Start now at " + Time.time + ">" + firstDelay + 2);
                TutorialNextStep(tutorialSteps[index]);
            }
        }
    }

    public void TutorialNextStep(TutorialStep step)
    {
        setText(step);
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
        player1.enabled = enabled;
        player2.enabled = enabled;
    }

    private void setText(TutorialStep step)
    {
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


    public void TutorialEnd()
    {
        Debug.Log("end of tutoriel !!!!!!");
        hasFinish = true;
        CloseDialog();
        UpdatePlayerInteraction(true);
    }

    public void StartLearningInteractionWithP1()
    {
        player1.enabled = true;
        player2.enabled = false;
        learnInteractionManager.InteractionWithP1();
    }
    public void StartLearningInteractionWithP2()
    {
        player1.enabled = false;
        player2.enabled = true;
        learnInteractionManager.InteractionWithP2();
    }
}

