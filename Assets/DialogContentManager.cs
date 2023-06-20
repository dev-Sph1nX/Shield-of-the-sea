using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogContentManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] TimeIndicator timeIndicator;
    [SerializeField] GameObject dialogGameObject;
    private IDialogManager dialogManager;

    private bool _isShow = false;
    private Animator animator;

    void Start()
    {
        dialogManager = dialogGameObject.GetComponent<IDialogManager>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetText(TutorialStep step)
    {
        if (!_isShow) // if hidden, let time to appear 
        {
            ShowDialog();
            StartCoroutine(UpdateData(step));
        }
        else
        {
            timeIndicator.StartTimer(step.forcedTimed ? true : step.invokeAction == null);
            dialogText.text = step.text;
        }
    }

    IEnumerator UpdateData(TutorialStep step)
    {
        yield return new WaitForSeconds(1f);
        timeIndicator.StartTimer(step.forcedTimed ? true : step.invokeAction == null);
        dialogText.text = step.text;
    }


    public void OnLocalNextStep()
    {
        dialogManager.OnNextStep();
    }

    public void ShowDialog()
    {
        _isShow = true;
        animator.SetTrigger("Open");

    }

    public void CloseDialog()
    {
        if (_isShow)
        {
            _isShow = false;
            animator.SetTrigger("Close");
            Invoke("ClearText", 2f);
        }
    }
    private void ClearText()
    {
        dialogText.text = "";
    }
}
