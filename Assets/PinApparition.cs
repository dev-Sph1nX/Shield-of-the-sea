using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinApparition : MonoBehaviour
{
    [SerializeField] Animator pinAnimator;

    public void Appear()
    {
        pinAnimator.SetTrigger("Appear");
    }

    public void Disappear()
    {
        pinAnimator.SetTrigger("Disappear");
    }
}
