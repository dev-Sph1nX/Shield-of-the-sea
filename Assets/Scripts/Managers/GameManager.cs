using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;



public class GameManager : MonoBehaviour
{
    [Header("Debug Mode")]
    [SerializeField] public bool debugMode = true;
    [ConditionalField("debugMode")] public string overrideSceneName = null;


    private static GameManager instance = null;
    public static GameManager Instance => instance;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        if (StaticClass.CrossSceneDebugMode)
        {
            debugMode = StaticClass.CrossSceneDebugMode;
        }
    }

    public void ChangeScene(string newSceneName, Action startAnimationMethod)
    {
        if (!isTransitioning)
        {
            startAnimationMethod();
            isTransitioning = true;
            StartCoroutine(LoadSceneAfterDelay((debugMode && overrideSceneName != null && overrideSceneName != "") ? overrideSceneName : newSceneName)); // 
        }
    }

    private IEnumerator LoadSceneAfterDelay(string newSceneName)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(newSceneName);
        isTransitioning = false;
    }

    public void FindNewPlayers()
    {
        PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
        foreach (PlayerMovement p in players)
        {
            if (p.playerId == SystemId.Player1)
            {
                WS ws = GetComponent<WS>();
                ws.player1 = p;
            }
            if (p.playerId == SystemId.Player2)
            {
                WS ws = GetComponent<WS>();
                ws.player2 = p;
            }
        }
    }
}