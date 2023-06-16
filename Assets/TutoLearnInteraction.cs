using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoLearnInteraction : MonoBehaviour
{
    [SerializeField] Animator p1Animator = null;
    [SerializeField] Animator p2Animator = null;

    private DialogManager dialogManager;
    private bool waitP1 = false, p1hasInteract = false;
    private bool waitP2 = false, p2hasInteract = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogManager = FindAnyObjectByType<DialogManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitP1 && p1hasInteract)
        {
            waitP1 = false;
            p1hasInteract = false;
            dialogManager.OnNextStep();
            // p1 has interact
        }
        if (waitP2 && p2hasInteract)
        {
            waitP2 = false;
            p2hasInteract = false;
            dialogManager.OnNextStep();
            // p2 has interact
        }
    }

    public void InteractionWithP1()
    {
        waitP1 = true;
    }

    public void InteractionWithP2()
    {
        waitP2 = true;
    }

    public void P1Interact()
    {
        if (waitP1)
        {
            p1hasInteract = true;
            p1Animator.SetTrigger("Wave");
        }
    }

    public void P2Interact()
    {
        if (waitP2)
        {
            p2hasInteract = true;
            p2Animator.SetTrigger("Wave");
        }
    }
}
