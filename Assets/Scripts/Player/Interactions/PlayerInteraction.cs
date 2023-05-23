using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] PlayerId playerId;
    [SerializeField] float interactRange = 3f;

    private bool Interaction()
    {
        if (playerId == PlayerId.Red)
        {
            return InputSystem.Player1Interaction();
        }
        else if (playerId == PlayerId.Blue)
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
            if (interactable != null)
            {
                interactable.Interact(transform);
            }
        }
    }


    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        Debug.Log(colliderArray);
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
