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
    [SerializeField] LevelManager levelManager;

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
        Debug.Log(shakeAmount + " // " + cameraDuration);
        cameraShake.shakeAmount = shakeAmount;
        cameraShake.shakeDuration = cameraDuration;
        // ps.Play();
        transform.DOMoveY(0, 2);

        foreach (GameObject ui in UIToHide)
        {
            ui.SetActive(false);
        }

        Invoke("UpdateUI", 2f);
    }

    void UpdateUI()
    {
        healthBarContainer.SetActive(true);

        // dispaly "ATTACK !" with animate
    }

    void BossDeath()
    {
        healthBarContainer.SetActive(false);
        _isShow = false;
        cameraShake.shakeAmount = shakeAmount;
        cameraShake.shakeDuration = cameraDuration;
        transform.DOMoveY(-3, 2);
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
