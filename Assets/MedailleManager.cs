using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

class Medaille
{
    public int score;
    public string text;
    public Color color;

    public Medaille(string pText, Color pColor, int pScore)
    {
        text = pText;
        color = pColor;
        score = pScore;
    }
}

public class MedailleManager : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI text;

    List<Medaille> medailles = new List<Medaille>();
    int index = 0;

    void Awake()
    {
        background.gameObject.SetActive(false);
    }

    void Start()
    {
        medailles.Add(new Medaille("OR", Color.yellow, 100));
        medailles.Add(new Medaille("ARGENT", Color.grey, 80));
        medailles.Add(new Medaille("BRONZE", new Color32(205, 127, 50, 255), 50));
        medailles.Add(new Medaille("CHOCOLAT", new Color32(210, 105, 30, 255), 0));
    }


    public void Show(float score)
    {
        background.gameObject.SetActive(true);
        UIUpdate();
    }

    public void OnScoreUpdate(float score)
    {
        Debug.Log(score + " < " + medailles[index].score);
        if (score < medailles[index].score)
        { // s'il est plus petit que la medaille actuel
            if (index + 1 < medailles.Count) // et qu'il reste des medailles en dessous
            {
                index++;
                UIUpdate();
            }
        }
    }
    void UIUpdate()
    {
        Medaille currentMedaille = medailles[index];
        text.text = currentMedaille.text;
        background.color = currentMedaille.color;
    }
}
