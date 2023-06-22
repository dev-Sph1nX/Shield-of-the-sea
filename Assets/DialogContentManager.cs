using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogContentManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] TimeIndicator timeIndicator;
    [SerializeField] GameObject dialogGameObject;
    public AudioClip typingClip;
    public AudioSourceGroup audioSourceGroup;

    private IDialogManager dialogManager;
    private DialogueVertexAnimator dialogueVertexAnimator;
    private bool _isShow = false;
    private Animator animator;
    private Coroutine typeRoutine = null;


    void Awake()
    {
        dialogueVertexAnimator = new DialogueVertexAnimator(dialogText, audioSourceGroup);

    }

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
            PlayDialogue(step.text);
            // dialogText.text = step.text;
        }
    }

    IEnumerator UpdateData(TutorialStep step)
    {
        yield return new WaitForSeconds(1f);
        timeIndicator.StartTimer(step.forcedTimed ? true : step.invokeAction == null);
        PlayDialogue(step.text);
        // dialogText.text = step.text;
    }

    void PlayDialogue(string message)
    {
        this.EnsureCoroutineStopped(ref typeRoutine);
        dialogueVertexAnimator.textAnimating = false;
        List<DialogueCommand> commands = DialogueUtility.ProcessInputString(message, out string totalTextMessage);
        typeRoutine = StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, typingClip, null));
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
