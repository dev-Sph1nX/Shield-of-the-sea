using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTarget : MonoBehaviour
{
    private TutoLearnWasteInteraction tutoLearnWasteInteraction;
    private CapsuleCollider capsuleCollider;
    private void Start()
    {
        tutoLearnWasteInteraction = FindAnyObjectByType<TutoLearnWasteInteraction>();
    }

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
            tutoLearnWasteInteraction.onP1Collide();
        }
        if (other.tag == "Player2")
        {
            tutoLearnWasteInteraction.onP2Collide();
        }
    }
}
