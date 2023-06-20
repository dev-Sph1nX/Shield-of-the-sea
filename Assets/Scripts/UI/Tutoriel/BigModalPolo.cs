using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigModalPolo : MonoBehaviour
{
    [SerializeField] int timer = 15;
    [SerializeField] TimeIndicator timeIndicator;

    private Animator animator;
    private DialogManager dialogManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        dialogManager = FindAnyObjectByType<DialogManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        Invoke("realShow", 1f);
    }

    private void realShow()
    {
        animator.SetTrigger("Open");
        Invoke("Close", timer);
        timeIndicator.StartTimer(true);
    }
    public void Close()
    {
        animator.SetTrigger("Close");
        Invoke("nextStep", 2f);
    }

    public void nextStep()
    {
        dialogManager.OnNextStep();
    }



}
