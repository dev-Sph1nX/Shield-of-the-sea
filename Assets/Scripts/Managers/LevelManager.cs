using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{

    [Header("Before Game")]
    [SerializeField][Range(3, 10)] public float timeToPlay = 3;
    [SerializeField][Range(0, 2)] public float countDownInterval = 1.25f;

    [Header("Game Time")]
    [SerializeField][Range(0, 120)] public float gameTime = 90f; // in seconds

    [Header("Points")]
    [SerializeField] private float percentage = 100;
    [SerializeField] public int percentageLostOnWasteLost = 2;
    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "0-Lobby";

    [Header("Reference")]
    [SerializeField] public GameObject canvas = null;
    [SerializeField] public TextMeshProUGUI startText;
    [SerializeField] public TextMeshProUGUI timeLeftText;
    [SerializeField] public WasteSpawner wasteSpawner;
    [SerializeField] public TextMeshProUGUI percentageText;
    [Header("Final modal ref")]
    [SerializeField] public GameObject finalModal;
    [SerializeField] public TextMeshProUGUI finalPercentageText;
    [SerializeField] public TextMeshProUGUI p1isReadyText;
    [SerializeField] public TextMeshProUGUI p2isReadyText;

    private Image mask = null;
    private Animator sceneAnimator = null;
    private bool isCountingDown = false, isPlaying = false, endCanvasIsShow = false;
    private bool p1haveInteract = false, p2haveInteract = false, isTransitioning = false;
    private float startTime;

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
            Debug.Log(gameTime);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Settrigger Enter");
        sceneAnimator.SetTrigger("Enter");
        Invoke("HideMask", 1f);
        // HideMask();
        startTime = Time.timeSinceLevelLoad;

        if (GameManager.Instance.debugMode)
        {
            timeToPlay = 4f;
            countDownInterval = 0.2f;
        }


        gameTime += timeToPlay + countDownInterval * 2;
    }

    private void HideMask()
    {
        Debug.Log("HideMask");

        mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        timeToPlay -= Time.timeSinceLevelLoad - startTime;
        gameTime -= Time.timeSinceLevelLoad - startTime;
        startTime = Time.timeSinceLevelLoad;

        if (timeToPlay < countDownInterval * 4 && !isCountingDown)
        {
            isCountingDown = true;
            StartCoroutine(CountDownCoroutine());
        }

        if (isPlaying)
        {
            DisplayTime(gameTime);
        }

        if (gameTime <= 0.0f && !endCanvasIsShow)
        {
            isPlaying = false;
            endCanvasIsShow = true;
            GameEnded();
        }

        if (endCanvasIsShow && !isTransitioning)
        {
            if (InputSystem.Player1Interaction())
            {
                p1haveInteract = !p1haveInteract;
                p1isReadyText.enabled = p1haveInteract;
            }

            if (InputSystem.Player2Interaction())
            {
                p2haveInteract = !p2haveInteract;
                p2isReadyText.enabled = p2haveInteract;
            }

            if (p1haveInteract && p2haveInteract)
            {
                isTransitioning = true;
                Debug.Log("chnage scene");
                GameManager.Instance.ChangeScene(nextSceneName, StartAnimation);
            }
        }
    }
    public void StartAnimation()
    {
        Debug.Log("set trigger on " + sceneAnimator.ToString());
        sceneAnimator.SetTrigger("Exit");
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

    void StartGame()
    {
        wasteSpawner.StartGame();
        UpdatePercentageDisplay();
        isPlaying = true;
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
            UpdatePercentageDisplay();
        }
    }

    public void UpdatePercentageDisplay()
    {
        percentageText.text = percentage.ToString();
    }

    public void GameEnded()
    {
        percentageText.text = "";
        timeLeftText.text = "";
        PlayerInteraction[] players = FindObjectsOfType<PlayerInteraction>();
        foreach (PlayerInteraction p in players)
        {
            p.enabled = false;
            p.gameObject.GetComponent<SimpleSampleCharacterControl>().GetStop();
            wasteSpawner.StopGame();
        }

        finalPercentageText.text = percentage + " %";
        finalModal.transform.localScale = Vector3.one;
    }
}
