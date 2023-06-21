using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HealthBar : MonoBehaviour
{
    private Image healthBarImage;

    public void Start()
    {
        healthBarImage = GetComponent<Image>();
    }

    public void UpdateHealthBar(float percentage)
    {
        float duration = 0.75f * (percentage / 100);
        healthBarImage.DOFillAmount(percentage / 100, duration);
    }

}
