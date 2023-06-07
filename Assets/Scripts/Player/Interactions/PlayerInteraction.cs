using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;


public class PlayerInteraction : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] public SystemId playerId;
    [SerializeField] float interactRange = 3f;
    [Header("Animation")]
    [SerializeField] string pickupTriggerName = "Pickup";
    Animator animator = null;
    public string objectId = NULL;
    const string NULL = "null";

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    // private void Update()
    // {
    //     if (Interaction())
    //     {
    //         OnUserInteract();
    //     }
    // }

    public void OnUserInteract()
    {
        Debug.Log("OnUserInteract of " + playerId);
        IInteractable interactable = GetInteractableObject();
        if (interactable != null)
        {
            animator.SetTrigger(pickupTriggerName);
            interactable.Interact(playerId);
        }

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
                interactableList.Add(interactable);
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

        // Prio 1 - La poubelle
        if (closestInteractable.Is<BinInteractable>())
        {
            return closestInteractable;
        }

        // Prio 2 - Object que l'on porte
        if (objectId != NULL)
        {
            return getObject();
        }

        // Else - Les objects par terre
        return closestInteractable;
    }

    public NPCInteractable getObject()
    {
        if (objectId != NULL)
        {
            NPCInteractable[] allWastes = FindObjectsOfType<NPCInteractable>();
            foreach (NPCInteractable w in allWastes)
            {
                if (w.GetId() == objectId) return w;
            }
        }
        return null;
    }
}
