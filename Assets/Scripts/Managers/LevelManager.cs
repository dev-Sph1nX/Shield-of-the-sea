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
    [SerializeField] public TextMeshProUGUI startText;
    [SerializeField] public TextMeshProUGUI timeLeftText;
    [SerializeField] public WasteSpawner wasteSpawner;
    [SerializeField] public HealthBar healthBar;

    [Header("Inital modal ref")]
    [SerializeField] public CustomModal initialModal;

    [Header("Final modal ref")]
    [SerializeField] public GameObject finalModal;
    [SerializeField] public TextMeshProUGUI finalPercentageText;
    [SerializeField] public TextMeshProUGUI p1isReadyText;
    [SerializeField] public TextMeshProUGUI p2isReadyText;
    [Header("Player Stat")]
    [SerializeField][ReadOnly] public int p1Score = 0;
    [SerializeField][ReadOnly] public int p2Score = 0;


    private Image mask = null;
    private Animator sceneAnimator = null;
    private bool isPlaying = false, endCanvasIsShow = false;
    private bool p1haveInteract = false, p2haveInteract = false, isTransitioning = false;
    private bool p1interact = false, p2interact = false, startGameTrigger = false;
    private float startTime = 0f;

    void Awake()
    {
        canvas.SetActive(true);
        sceneAnimator = canvas.GetComponent<Animator>();
        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject.tag == "TransitionImage")
                mask = child.gameObject.GetComponent<Image>();
        }
        if (StaticClass.CrossSceneGameTime > 0)
        {
            gameTime = StaticClass.CrossSceneGameTime;
        }

        GameManager.Instance.FindNewPlayers();
        GameManager.Instance.FindLevelManager();
    }

    void Start()
    {
        sceneAnimator.SetTrigger("Enter");
        Invoke("HideMask", 1f);
        Invoke("OpenInitialModal", 1f);

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

        // after end game 
        if (endCanvasIsShow && !isTransitioning)
        {
            if (p1interact || InputSystem.Player1Interaction())
            {
                p1interact = false;
                p1haveInteract = !p1haveInteract;
                p1isReadyText.enabled = p1haveInteract;
            }

            if (p2interact || InputSystem.Player2Interaction())
            {
                p2interact = false;
                p2haveInteract = !p2haveInteract;
                p2isReadyText.enabled = p2haveInteract;
            }

            if (p1haveInteract && p2haveInteract)
            {
                isTransitioning = true;
                ChangeScene();
            }
        }
    }


    public void onOpeningModalClose()
    {
        startGameTrigger = true;
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
        healthBar.UpdateHealthBar(percentage);
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
        endCanvasIsShow = true;
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
            healthBar.UpdateHealthBar(percentage);
        }
    }

    public void GameEnded()
    {
        timeLeftText.text = "";
        PlayerInteraction[] players = FindObjectsOfType<PlayerInteraction>();
        foreach (PlayerInteraction p in players)
        {
            p.enabled = false;
            p.gameObject.GetComponent<SimpleSampleCharacterControl>().GetStop();
        }
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
        wasteSpawner.StopGame();

        finalPercentageText.text = percentage + " %";
        finalModal.transform.localScale = Vector3.one;
    }

    public void ChangeScene()
    {
        GameManager.Instance.ChangeScene(nextSceneName, StartAnimation);
    }

    public void StartAnimation()
    {
        sceneAnimator.SetTrigger("Exit");
    }

    public void OnPlayer1Interaction(bool scoring)
    {
        if (!isPlaying)
        {
            initialModal.Player1Interact();
        }

        if (scoring)
        {
            p1Score++;
        }

        if (endCanvasIsShow && !isTransitioning) // for the final modal
        {
            p1interact = true;
        }
    }
    public void OnPlayer2Interaction(bool scoring)
    {
        if (!isPlaying)
        {
            initialModal.Player2Interact();
        }
        if (scoring)
        {
            p2Score++;
        }
        if (endCanvasIsShow && !isTransitioning)
        {
            p2interact = true;
        }

    }
}
