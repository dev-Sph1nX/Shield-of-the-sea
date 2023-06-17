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
    private float currentTime;
    private bool updateTime;

    // Update is called once per frame
    private void Update()
    {
        if (updateTime)
        {
            currentTime -= Time.deltaTime; if (currentTime <= 0.0f)
            {
                // Stop the countdown timer              
                updateTime = false;
                currentTime = 0.0f;
                dialogManager.OnNextStep();
            }
            float normalizedValue = Mathf.Clamp(currentTime / timeBetweenText, 0.0f, 1.0f);
            image.fillAmount = normalizedValue;
        }
    }

    public void StartTimer(bool timed)
    {
        // should depend of newText lenght
        // but here it's always timeBetweenText
        if (timed)
        {
            currentTime = timeBetweenText;
            image.fillAmount = 1.0f;
            updateTime = true;
        }
        else
            image.fillAmount = 0;
    }

    // private IEnumerator CountDown()
    // {
    //     for (float i = nbFrame; i >= 0; i--)
    //     {
    //         yield return new WaitForSeconds(timeBetweenText / nbFrame);
    //         image.fillAmount = i / 100;
    //     }

    //     dialogManager.OnNextStep();
    // }
}
