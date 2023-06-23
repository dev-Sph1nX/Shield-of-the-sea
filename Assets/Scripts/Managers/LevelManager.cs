using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyBox;

public class LevelManager : MonoBehaviour
{

    [Header("Before Game")]
    // [SerializeField][Range(3, 10)] public float timeToPlay = 3;
    [SerializeField][Range(0, 2)] public float countDownInterval = 1.25f;

    [Header("Game Time")]
    [SerializeField][Range(0, 120)] public float gameTime = 90f; // in seconds

    [Header("Points")]
    [SerializeField] public float percentage = 100;
    [SerializeField] public int percentageLostOnWasteLost = 2;
    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "0-Lobby";

    [Header("Reference")]
    [SerializeField] public GameObject canvas = null;
    [SerializeField] public Animator cameraAnimator;

    [SerializeField] public TextMeshProUGUI startText;
    [SerializeField] public TextMeshProUGUI timeLeftText;
    [SerializeField] public WasteSpawner wasteSpawner;
    [SerializeField] public TextMeshProUGUI p1ScoreText;
    [SerializeField] public TextMeshProUGUI p2ScoreText;
    [SerializeField] public MedailleManager medailleManager;
    [SerializeField] public FinalBoss finalBoss;

    [Header("Inital modal ref")]
    [SerializeField] public CustomModal initialModal;

    [Header("Final modal ref")]
    [SerializeField] public CustomModal finalModal;

    [Header("Player name ref")]
    [SerializeField] PlayerNameApparition player1NameApparition;
    [SerializeField] PlayerNameApparition player2NameApparition;
    private int p1Score = 0;
    private int p2Score = 0;


    private Image mask = null;
    private Animator sceneAnimator = null;
    private bool isPlaying = false, endCanvasIsShow = false;
    private bool startGameTrigger = false;
    private float startTime = 0f, tiersOfGameTime, nextTiers = 0f, actualGameTime;

    void Awake()
    {
        canvas.SetActive(true);
        sceneAnimator = canvas.GetComponent<Animator>();
        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject.tag == "TransitionImage")
                mask = child.gameObject.GetComponent<Image>();
        }

        GameManager.Instance.FindNewPlayers();
        GameManager.Instance.FindLevelManager();
    }

    void Start()
    {
        tiersOfGameTime = gameTime / 3;
        nextTiers = tiersOfGameTime;

        sceneAnimator.SetTrigger("Enter");
        cameraAnimator.SetTrigger("Enter");
        Invoke("HideMask", 1f);
        Invoke("OpenInitialModal", 2f);

        // if (GameManager.Instance.debugMode)
        // {
        //     timeToPlay = 4f;
        //     countDownInterval = 0.2f;
        // }
    }

    private void HideMask()
    {
        mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 0f);
    }
    private void OpenInitialModal()
    {
        initialModal.ShowModal();
    }

    // Update is called once per frame
    void Update()
    {
        // is trigger on close of opening level modal 
        if (startGameTrigger)
        {
            startGameTrigger = false;
            StartCoroutine(CountDownCoroutine());
        }

        if (isPlaying)
        {
            actualGameTime = Time.timeSinceLevelLoad - startTime;
            if (actualGameTime > nextTiers)
            {
                nextTiers += tiersOfGameTime;
                wasteSpawner.IncrementIntensity();
            }
        }
    }


    public void onOpeningModalClose()
    {
        startGameTrigger = true;

        player1NameApparition.Appear();
        player2NameApparition.Appear();
    }

    private System.Collections.IEnumerator CountDownCoroutine()
    {
        for (int i = 3; i >= 0; i--)
        {
            yield return new WaitForSeconds(countDownInterval);
            if (i == 0)
            {
                startText.text = "Jouez !";
                Invoke("HideText", countDownInterval);
            }
            else
            {
                startText.text = i.ToString();
            }
        }
    }


    void HideText()
    {
        startText.gameObject.SetActive(false);
        Invoke("StartGame", 1f);
    }

    public void StartGame()
    {
        wasteSpawner.StartGame(gameTime);
        medailleManager.Show(percentage);
        // healthBar.UpdateHealthBar(percentage);
        isPlaying = true;
        startTime = Time.timeSinceLevelLoad;

        StartCoroutine("GameTimerCoroutine");
    }
    private System.Collections.IEnumerator GameTimerCoroutine()
    {
        while (gameTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            gameTime--;
            DisplayTime(gameTime);
        }
        StopCoroutine("GameTimerCoroutine");
        GameEnded();
        isPlaying = false;
    }



    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeLeftText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void OnWasteLost()
    {
        if (isPlaying && percentage - percentageLostOnWasteLost >= 0)
        {
            percentage -= percentageLostOnWasteLost;
            // healthBar.UpdateHealthBar(percentage);
            medailleManager.OnScoreUpdate(percentage);
        }
    }

    public void GameEnded()
    {
        NPCInteractable[] wastes = FindObjectsOfType<NPCInteractable>();
        foreach (NPCInteractable w in wastes)
        {
            WasteShadow shadowManager = w.GetComponent<WasteShadow>();
            if (shadowManager)
            {
                shadowManager.DestroyShadow();
            }
            if (w.gameObject.tag != "Boss")
            {
                Destroy(w.gameObject);
            }
        }
        wasteSpawner.StopGame();
        finalBoss.Appear();
    }

    public void OnBossDeath()
    {
        endCanvasIsShow = true;
        PlayerInteraction[] players = FindObjectsOfType<PlayerInteraction>();
        foreach (PlayerInteraction p in players)
        {
            p.gameObject.GetComponent<SimpleSampleCharacterControl>().GetStop();
        }
        finalModal.ShowModal();
    }

    public string getFinalPercentage()
    {
        return percentage + " %";
    }

    // called after final modal is done 
    public void ChangeScene()
    {
        cameraAnimator.SetTrigger("Exit");
        Invoke("GameManagerChangeScene", 1f);
    }

    void GameManagerChangeScene()
    {
        GameManager.Instance.ChangeScene(nextSceneName, StartAnimation);
    }

    public void StartAnimation()
    {
        sceneAnimator.SetTrigger("Exit");
    }

    public void OnPlayer1Interaction(bool scoring)
    {
        if (!isPlaying && !endCanvasIsShow)
        {
            initialModal.Player1Interact();
        }

        if (scoring)
        {
            p1Score++;
            p1ScoreText.text = p1Score.ToString();
        }

        if (!isPlaying && endCanvasIsShow)
        {
            finalModal.Player1Interact();
        }
    }
    public void OnPlayer2Interaction(bool scoring)
    {
        if (!isPlaying && !endCanvasIsShow)
        {
            initialModal.Player2Interact();
        }
        if (scoring)
        {
            p2Score++;
            p2ScoreText.text = p2Score.ToString();
        }
        if (!isPlaying && endCanvasIsShow)
        {
            finalModal.Player2Interact();
        }

    }
}
