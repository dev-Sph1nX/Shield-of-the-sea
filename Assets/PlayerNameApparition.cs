using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameApparition : MonoBehaviour
{
    [SerializeField] Animator nameAnimator;
    [SerializeField] float timer;

    public void Appear()
    {
        nameAnimator.SetTrigger("Appear");
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timer);
        nameAnimator.SetTrigger("Disappear");
    }
}
