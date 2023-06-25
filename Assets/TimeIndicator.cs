using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeIndicator : MonoBehaviour
{
    [SerializeField] int timeBetweenText = 5;
    [SerializeField] Image image;
    [SerializeField] DialogContentManager dialogContentManager;
    // Start is called before the first frame update

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
                // if (dialogContentManager)
                //     dialogContentManager.OnLocalNextStep();
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
}

