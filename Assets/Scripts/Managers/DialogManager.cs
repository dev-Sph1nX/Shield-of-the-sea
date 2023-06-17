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
    [SerializeField] TutoLearnWasteInteraction learnWasteInteractionManager;
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
        tutorialSteps.Add(new TutorialStep("Bienvenue dans WAKE ! Ça va secouer !"));
        tutorialSteps.Add(new TutorialStep("Vous allez apprendre à vous déplacer."));
        tutorialSteps.Add(new TutorialStep("Chacun doit aller se déplacer sur la cible de votre couleur.", learnMovementManager.MovetoTarget1));
        tutorialSteps.Add(new TutorialStep("Super ! Vous pouvez continuer.", learnMovementManager.MovetoTarget2));
        tutorialSteps.Add(new TutorialStep("Encore un petit tour de manège !", learnMovementManager.MovetoTarget3));
        tutorialSteps.Add(new TutorialStep("Et un autre", learnMovementManager.MovetoTarget4));
        tutorialSteps.Add(new TutorialStep("Allez juste un petit dernier", learnMovementManager.MovetoTarget5));
        tutorialSteps.Add(new TutorialStep("Bien normalement, vous savez vous déplacer maintenant"));
        tutorialSteps.Add(new TutorialStep("On va passer à comment on interagit avec le jeu."));
        tutorialSteps.Add(new TutorialStep("Regarde, il suffit d'agiter ton épée comme ça. (il y aura surement une vidéo à montrer)"));
        tutorialSteps.Add(new TutorialStep("Vas-y, essaye joueur 1 ! (Le rouge :)", StartLearningInteractionWithP1));
        tutorialSteps.Add(new TutorialStep("Bien joué ! "));
        tutorialSteps.Add(new TutorialStep("À ton tour, joueur 2. (Le bleu :)", StartLearningInteractionWithP2));
        tutorialSteps.Add(new TutorialStep("Super ! "));
        tutorialSteps.Add(new TutorialStep("On va parler du principe de base du gameplay : les déchets"));
        tutorialSteps.Add(new TutorialStep("Ils arriveront en parabole pour le bord de l'écran et vous devrez les détruire avant qu'il ne s'enfonce trop dans le sable."));
        tutorialSteps.Add(new TutorialStep("Place à l'exemple : on précise qu'ici le temps est un peu accéléré, ne vous inquiété pas le déchet s'enfoncera moins vite dans le sable en jeu", learnWasteInteractionManager.throwOneWaste));
        tutorialSteps.Add(new TutorialStep("Pour éviter de perdre des points, vous devez donc les détruire. Pour ce faire, il faut vous déplacer à proximité du déchet et donner un coup d'épée comme appris précédemment."));
        tutorialSteps.Add(new TutorialStep("Joueur 1, ça va être à votre tour. Détruit le déchet qui arrive", throwForP1));
        tutorialSteps.Add(new TutorialStep("À toi joueur 2", throwForP2));
        tutorialSteps.Add(new TutorialStep("Maintenant, ça va être les deux en même temps.", throwForBoth));
        tutorialSteps.Add(new TutorialStep("Et voilà le tuto est terminé. "));
        tutorialSteps.Add(new TutorialStep("Pour commencer le jeu, il faudra vous mettre prêt. Pour être prêt, rien de plus simple : donner un grand coup d'épée ! "));
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

    public void throwForP1()
    {
        player1.enabled = true;
        player2.enabled = false;
        learnWasteInteractionManager.throwForP1();
    }
    public void throwForP2()
    {
        player1.enabled = false;
        player2.enabled = true;
        learnWasteInteractionManager.throwForP2();
    }

    public void throwForBoth()
    {
        player1.enabled = true;
        player2.enabled = true;
        learnWasteInteractionManager.throwForBoth();
    }

}

