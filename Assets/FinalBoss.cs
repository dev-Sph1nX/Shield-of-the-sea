using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FinalBoss : MonoBehaviour, IInteractable
{
    [Header("Gameplay")]
    [SerializeField] int playerDamage;
    [SerializeField] int healthPercentage = 100;

    [Header("Reference")]
    [SerializeField] CameraShake cameraShake;
    [SerializeField] ParticleSystem deathParticule;
    [SerializeField] LevelManager levelManager;

    [Header("UI Reference")]
    [SerializeField] GameObject healthBarContainer;
    [SerializeField] GameObject[] UIToHide;
    // private ParticleSystem ps;
    private HealthBar healthBar;
    private bool p1trigger = false, p2trigger = false;
    private bool _isShow = false;

    void Start()
    {
        healthBar = healthBarContainer.GetComponentInChildren<HealthBar>();
        // ps = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        // if (InputSystem.getButton("Fire1")) Appear();
    }

    public void Appear()
    {
        _isShow = true;
        cameraShake.shakeDuration = 2;
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
        cameraShake.shakeDuration = 2;
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
}
