using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [Header("Type")]
    [SerializeField] public SystemId typeId;

    [Header("References")]

    [SerializeField] private ParticleSystem hitPs;

    private SystemId? ownerId = null;
    private MeshRenderer child;
    private bool isInteracted = false;
    private TutoLearnWasteInteraction tutoLearnWasteInteraction;

    private void Awake()
    {
        child = GetComponentInChildren<MeshRenderer>();
        tutoLearnWasteInteraction = FindAnyObjectByType<TutoLearnWasteInteraction>();
    }

    private void Update()
    {
    }

    public void Interact(SystemId id)
    {
        if (!isInteracted)
        {
            isInteracted = true;
            hitPs.Play();
            Destroy(child.gameObject);
            Invoke("Destroy", .5f);
            if (tutoLearnWasteInteraction) tutoLearnWasteInteraction.onWasteDestroyByPlayer(typeId);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    public Transform GetTransform()
    {
        return transform;
    }
    public IconType GetInteractIcon()
    {
        return ownerId != null ? IconType.Release : IconType.Take;
    }

    public bool isInteractable()
    {
        return !isInteracted;
    }
}