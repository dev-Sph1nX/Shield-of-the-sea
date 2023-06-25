using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MyBox;

public class FinalBoss : MonoBehaviour, IInteractable
{
    [Header("Gameplay")]
    [SerializeField] int playerDamage;
    [SerializeField] int healthPercentage = 100;

    [Header("Camera shake")]
    [SerializeField] public float cameraDuration;
    [SerializeField] public float shakeAmount;

    [Header("Reference")]
    [SerializeField] CameraShake cameraShake;
    [SerializeField] ParticleSystem deathParticule;
    [SerializeField] AudioSource spawnSound;
    [SerializeField] AudioSource bossMusic;
    [SerializeField] LevelManager levelManager;
    [SerializeField] Animator annoucementAnimator;
    [SerializeField] PlayerInteraction player1;
    [SerializeField] PlayerInteraction player2;
    [SerializeField] PinApparition pinPlayer1Animator;
    [SerializeField] PinApparition pinPlayer2Animator;

    [Header("UI Reference")]
    [SerializeField] GameObject healthBarContainer;
    [SerializeField] GameObject[] UIToHide;
    [SerializeField][ReadOnly] private string id;

    // private ParticleSystem ps;
    private HealthBar healthBar;
    private bool _isShow = false;

    void Awake()
    {
        id = Helpers.generateId();
    }

    void Start()
    {
        healthBar = healthBarContainer.GetComponentInChildren<HealthBar>();
        // ps = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {

        if (player1.objectId == id)
        {
            pinPlayer1Animator.Appear();
        }
        else
        {
            pinPlayer1Animator.Disappear();

        }

        if (player2.objectId == id)
        {
            pinPlayer2Animator.Appear();
        }
        else
        {
            pinPlayer2Animator.Disappear();
        }

        // if (InputSystem.getButton("Fire1")) Appear();

        // PlayerInteraction[] players = FindObjectsOfType<PlayerInteraction>();
        // foreach (PlayerInteraction p in players)
        // {
        //     if (p.objectId == id)
        //     {
        //         ActiveOutline();
        //     }
        //     else
        //     {
        //         DisableOutline();
        //     }
        // }

    }

    public void Appear()
    {
        _isShow = true;
        cameraShake.shakeAmount = shakeAmount;
        cameraShake.shakeDuration = cameraDuration;
        // ps.Play();
        transform.DOMoveY(0, 2);
        spawnSound.Play();

        foreach (GameObject ui in UIToHide)
        {
            ui.SetActive(false);
        }

        Invoke("UpdateUI", 2f);
    }

    void UpdateUI()
    {
        healthBarContainer.SetActive(true);
        annoucementAnimator.gameObject.SetActive(true);
        annoucementAnimator.SetTrigger("Show");
        bossMusic.Play();
        bossMusic.DOFade(bossMusic.volume, 1);
    }

    void BossDeath()
    {
        bossMusic.DOFade(0, 1);

        healthBarContainer.SetActive(false);
        annoucementAnimator.SetTrigger("Hide");

        _isShow = false;
        cameraShake.shakeAmount = shakeAmount;
        cameraShake.shakeDuration = cameraDuration;
        transform.DOMoveY(-3, 2);
        spawnSound.Play();
        Invoke("CallLevelManager", 2f);
    }

    void CallLevelManager()
    {
        levelManager.OnBossDeath();
    }

    public void Interact(SystemId id)
    {
        if (healthPercentage - playerDamage > 0)
        {
            healthPercentage -= playerDamage;
            healthBar.UpdateHealthBar(healthPercentage);
        }
        else
        {
            BossDeath();
        }

    }

    public Transform GetTransform()
    {
        return transform;
    }
    public IconType GetInteractIcon()
    {
        return IconType.Release;
    }

    public bool isInteractable()
    {
        return _isShow;
    }

    public string getId()
    {
        return id;
    }
}
