using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{

    private Animator animator;
    private bool isTaken;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        // npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    public void Interact(Transform interactorTransform)
    {
        // ChatBubble3D.Create(transform.transform, new Vector3(-.3f, 1.7f, 0f), ChatBubble3D.IconType.Happy, "Hello there!");

        Debug.Log("on interact");

    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    public Transform GetTransform()
    {
        return transform;
    }
    public IconType GetInteractIcon()
    {
        return isTaken ? IconType.Release : IconType.Take;
    }

}