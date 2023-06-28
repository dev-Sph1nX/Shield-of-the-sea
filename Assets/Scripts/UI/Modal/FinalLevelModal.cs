using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class FinalLevelModal : MonoBehaviour, InnerModalScript
{
    [SerializeField] public TextMeshProUGUI finalPercentageText1;
    [SerializeField] public TextMeshProUGUI finalPercentageText2;
    [SerializeField] public TextMeshProUGUI nbBottle;
    [SerializeField] public TextMeshProUGUI nbPaper;
    [SerializeField] public TextMeshProUGUI nbPneu;
    [SerializeField] public Image medaille;
    [SerializeField] public Sprite goldSprite;
    [SerializeField] public Sprite silverSprite;
    [SerializeField] public Sprite bronzeSprite;
    [SerializeField] public Sprite chocoSprite;
    [SerializeField] HealthBar healthBar;
    [SerializeField] TimeIndicator timeIndicator;
    [SerializeField] GameObject[] UITHide;

    [SerializeField] LevelManager levelManager;

    private bool _isDone = false;
    float percentage, tweenValue = 0;

    int maxPointer = 405;
    int minPointer = -439;

    public bool isDone()
    {
        return _isDone;
    }
    public void isShow()
    {

        Invoke("HideUI", 1f);

        percentage = levelManager.getFinalPercentage();
        Debug.Log("percentage" + percentage);

        nbBottle.text = "x" + levelManager.getFinalP1Score();
        nbPaper.text = "x" + levelManager.getFinalP2Score();
        nbPneu.text = "x" + levelManager.getFinalPneuScore();

        timeIndicator.StartTimer(true);
        Invoke("CloseModal", 10);
    }

    void HideUI()
    {
        DOTween.To(() => tweenValue, x => tweenValue = x, percentage, 3).SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            medaille.sprite = getMedailleSprite(tweenValue);
            finalPercentageText1.text = (int)tweenValue + "%";
            finalPercentageText2.text = (int)tweenValue + "%";
        });

        healthBar.UpdateHealthBar(percentage, 3);
        foreach (GameObject obj in UITHide)
        {
            obj.SetActive(false);
        }
    }

    void CloseModal()
    {
        foreach (GameObject obj in UITHide)
        {
            obj.SetActive(true);
        }
        _isDone = true;
    }


    public void OnPlayer1Interact()
    {
    }
    public void OnPlayer2Interact()
    {
    }

    Sprite getMedailleSprite(float score)
    {
        if (score == 100)
        {
            return goldSprite;
        }
        else if (score > 84)
        {
            return silverSprite;
        }
        else if (score > 52)
        {
            return bronzeSprite;
        }
        else
        {
            return chocoSprite;
        }
    }
}