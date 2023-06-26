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
    [SerializeField] public Image pointer;
    [SerializeField] public Image medaille;
    [SerializeField] public Sprite goldSprite;
    [SerializeField] public Sprite silverSprite;
    [SerializeField] public Sprite bronzeSprite;
    [SerializeField] public Sprite chocoSprite;
    [SerializeField] HealthBar healthBar;
    [SerializeField] TimeIndicator timeIndicator;
    [SerializeField] GameObject[] UITHide;

    [SerializeField] LevelManager levelManager;

    private bool _isDone = false, _isShow = true, p1interact = false, p2interact = false;
    float percentage;

    int maxPointer = 405;
    int minPointer = -439;
    void Update()
    {
        if (_isShow)
        {
            // if (p1interact)
            // {
            //     p1text.enabled = true;
            // }
            // if (p2interact)
            // {
            //     p2text.enabled = true;
            // }

            if (p1interact && p2interact)
            {
                _isDone = true;
            }
        }
    }

    public bool isDone()
    {
        return _isDone;
    }
    public void isShow()
    {

        Invoke("HideUI", 1f);


        _isShow = true;
        percentage = levelManager.getFinalPercentage();
        Debug.Log("percentage" + percentage);

        nbBottle.text = "x" + levelManager.getFinalP1Score();
        nbPaper.text = "x" + levelManager.getFinalP2Score();

        finalPercentageText1.text = percentage + "%";
        finalPercentageText2.text = percentage + "%";

        medaille.sprite = getMedailleSprite(percentage);

        float delta = (maxPointer - minPointer) * (percentage / 100);
        Vector3 endPosition = new Vector3(minPointer + delta, pointer.transform.position.y, pointer.transform.position.z);
        pointer.transform.DOMove(endPosition, 4);

        healthBar.UpdateHealthBar(percentage, 4);

        timeIndicator.StartTimer(true);
        Invoke("CloseModal", 10);
    }

    void HideUI()
    {
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
        p1interact = true;
    }
    public void OnPlayer2Interact()
    {
        p2interact = true;
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