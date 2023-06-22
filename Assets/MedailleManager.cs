using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

class Medaille
{
    public int score;
    public Image image;

    public Medaille(Image pImage, int pScore)
    {
        image = pImage;
        score = pScore;
    }
}

public class MedailleManager : MonoBehaviour
{
    [Header("Medailles assets")]
    [SerializeField] Image goldMedaille;
    [SerializeField] Image silverMedaille;
    [SerializeField] Image bronzeMedaille;
    [SerializeField] Image chocolateMedaille;
    [Header("Reference")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] HealthBar healthBar;

    List<Medaille> medailles = new List<Medaille>();
    int index = 0;

    void Awake()
    {
        canvasGroup.alpha = 0;
    }

    void Start()
    {
        medailles.Add(new Medaille(goldMedaille, 100));
        medailles.Add(new Medaille(silverMedaille, 80));
        medailles.Add(new Medaille(bronzeMedaille, 50));
        medailles.Add(new Medaille(chocolateMedaille, 0));
    }


    public void Show(float score)
    {
        canvasGroup.alpha = 1;
    }

    public void OnScoreUpdate(float score)
    {
        healthBar.UpdateHealthBar(score);
        // Debug.Log(score + " < " + medailles[index].score);
        if (score < medailles[index].score)
        { // s'il est plus petit que la medaille actuel
            if (index + 1 < medailles.Count) // et qu'il reste des medailles en dessous
            {
                Debug.Log("launch anim");
                medailles[index].image.gameObject.GetComponent<Animator>().SetTrigger("Swap");
                index++;
            }
        }
    }

    // void UpdateMedaille()
    // {
    //     Debug.Log("update medailel");

    //     Medaille currentMedaille = medailles[index];
    //     medailleContainer.sprite = currentMedaille.sprite;

    //     Medaille behindMedaille = medailles[index + 1];
    //     medailleBehindContainer.sprite = behindMedaille.sprite;
    // }


}
