using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] PlayerId playerId;
    [SerializeField] float interactRange = 3f;
    [Header("Animation")]
    [SerializeField] string pickupTriggerName = "Pickup";
    Animator animator = null;
    public string objectId = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private bool Interaction()
    {
        if (playerId == PlayerId.Player1)
        {
            return InputSystem.Player1Interaction();
        }
        else if (playerId == PlayerId.Player2)
        {
            return InputSystem.Player2Interaction();
        }
        return false;
    }

    private void Update()
    {
        if (Interaction())
        {
            IInteractable interactable = GetInteractableObject();
            Debug.Log("objectId : " + objectId);
            Debug.Log("interactable?.GetId() : " + interactable?.GetId());
            if (interactable != null && (objectId == null || objectId == interactable?.GetId()))
            {
                if (objectId == null)
                {
                    objectId = interactable.GetId();
                }
                else
                {
                    objectId = null;
                }
                animator.SetTrigger(pickupTriggerName);
                interactable.Interact(playerId);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    public IInteractable GetInteractableObject()
    {
        if (objectId != null)
        {
            NPCInteractable[] allWastes = FindObjectsOfType<NPCInteractable>();
            foreach (NPCInteractable w in allWastes)
            {
                if (w.GetId() == objectId) return w;
            }
        }

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

        return closestInteractable;
    }
}
