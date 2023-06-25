using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;
using System.Linq;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] public SystemId playerId;
    [SerializeField] float interactRange = 3f;
    [Header("Animation")]
    [SerializeField] string pickupTriggerName = "Pickup";
    [Header("Camera shake on interaction")]
    [SerializeField] public float cameraDuration;
    [SerializeField] public float shakeAmount;
    [Header("Reference")]
    [SerializeField] public CameraShake cameraShake;
    [SerializeField] public AudioSource swordSound;


    Animator animator = null;
    public string objectId = NULL;
    const string NULL = "null";
    private string sceneName;
    private LobbyManager lobbyManager;
    private TutoLearnWasteInteraction tutoLearnWasteInteraction;
    public LevelManager levelManager;
    private bool interact = false, canInteract = true;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        lobbyManager = FindObjectOfType<LobbyManager>();
        tutoLearnWasteInteraction = FindObjectOfType<TutoLearnWasteInteraction>();
        levelManager = FindObjectOfType<LevelManager>();
        sceneName = SceneManager.GetActiveScene().name;
    }

    private bool Interaction()
    {
        if (playerId == SystemId.Player1)
        {
            return InputSystem.Player1Interaction();
        }
        else if (playerId == SystemId.Player2)
        {
            return InputSystem.Player2Interaction();
        }
        return false;
    }

    private void Update()
    {
        if (interact || (GameManager.Instance.debugMode && Interaction()))
        {
            interact = false;
            if (canInteract)
                OnUserInteract();
        }
        GetInteractableObject(); // here for real time interact indicator
    }

    public void OnUserInteract()
    {
        IInteractable interactable = GetInteractableObject();
        if (sceneName == "0-Lobby")
        {
            if (playerId == SystemId.Player1)
            {
                tutoLearnWasteInteraction.onWasteDestroyByPlayer1();
            }
            if (playerId == SystemId.Player2)
            {
                tutoLearnWasteInteraction.onWasteDestroyByPlayer2();
            }
        }
        if (sceneName == "3-Beach")
        {
            if (playerId == SystemId.Player1)
            {
                levelManager.OnPlayer1Interaction(interactable != null);
            }
            if (playerId == SystemId.Player2)
            {
                levelManager.OnPlayer2Interaction(interactable != null);
            }
        }
        animator.SetTrigger(pickupTriggerName);
        swordSound.Play();
        if (interactable != null)
        {
            cameraShake.shakeAmount = shakeAmount;
            cameraShake.shakeDuration = cameraDuration;
            interactable.Interact(playerId);
        }
    }


    public void GetInteractionFromWS()
    {
        interact = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    public IInteractable GetInteractableObject()
    {

        List<IInteractable> interactableList = new List<IInteractable>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.isInteractable() && isAllowedToInteract(interactable))
                {
                    interactableList.Add(interactable);
                }
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    // Closer
                    closestInteractable = interactable;
                }
            }
        }

        objectId = closestInteractable?.getId() ?? null;
        // Else - Les objects par terre
        return closestInteractable;
    }


    public bool isAllowedToInteract(IInteractable interactable)
    {
        SystemId wasteId = interactable.GetTransform().gameObject.GetComponent<NPCInteractable>().typeId;
        if (playerId == SystemId.Player1 && wasteId == SystemId.Cannette)
        {
            return true;
        }
        if (playerId == SystemId.Player2 && wasteId == SystemId.Glass)
        {
            return true;
        }
        if (wasteId == SystemId.Pneu) return true;
        if (wasteId == SystemId.Boss) return true;
        return false;
    }

    public void SetCanInteract(bool can)
    {
        canInteract = can;
    }
}
