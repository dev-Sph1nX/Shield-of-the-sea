using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomModal : MonoBehaviour
{
    public GameObject innerModalScriptObject;
    private InnerModalScript innerModalScript;


    [SerializeField] public UnityEvent onDone;

    private Animator animator;
    private bool localIsDone = false;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        innerModalScript = innerModalScriptObject.GetComponent<InnerModalScript>();
    }


    // Update is called once per frame
    void Update()
    {
        if (innerModalScript.isDone() && !localIsDone)
        {
            localIsDone = true;
            animator.SetTrigger("Close");
            onDone.Invoke();
        }
    }

    public void ShowModal()
    {
        animator.SetTrigger("Open");
    }
    public void Player1Interact()
    {
        innerModalScript.OnPlayer1Interact();
    }
    public void Player2Interact()
    {
        innerModalScript.OnPlayer2Interact();
    }
}

public interface InnerModalScript
{
    void isShow();
    bool isDone();
    void OnPlayer1Interact();
    void OnPlayer2Interact();
}