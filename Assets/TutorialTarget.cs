using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTarget : MonoBehaviour
{
    [SerializeField] public UnityEvent onP1Collider;
    [SerializeField] public UnityEvent onP2Collider;
    private CapsuleCollider capsuleCollider;

    private void OnTriggerEnter(Collider other)
    {
        detection(other);
    }
    private void OnTriggerExit(Collider other)
    {
        detection(other);
    }

    private void detection(Collider other)
    {
        if (other.tag == "Player1")
        {
            onP1Collider.Invoke();
        }
        if (other.tag == "Player2")
        {
            onP2Collider.Invoke();
        }
    }
}
