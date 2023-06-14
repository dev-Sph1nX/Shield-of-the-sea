using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeIndicator : MonoBehaviour
{
    [SerializeField] int timeBetweenText = 5;
    [SerializeField] Image image;
    [SerializeField] DialogManager dialogManager;
    // Start is called before the first frame update

    private int nbFrame = 100;


    // Update is called once per frame
    void Update()
    {

    }

    public void StartTimer(bool timed)
    {
        // should depend of newText lenght
        // but here it's always timeBetweenText
        if (timed)
            StartCoroutine(CountDown());
        else
            image.fillAmount = 0;
    }

    private IEnumerator CountDown()
    {
        for (float i = nbFrame; i >= 0; i--)
        {
            yield return new WaitForSeconds(timeBetweenText / nbFrame);
            image.fillAmount = i / 100;
        }

        dialogManager.OnNextStep();
    }
}
