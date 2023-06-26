using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System.Linq;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [Header("Type")]
    [SerializeField] public SystemId typeId;

    [Header("References")]

    [SerializeField] private ParticleSystem hitPs;
    [SerializeField] private AudioSource hitSound;
    [SerializeField][ReadOnly] private string id;

    private SystemId? ownerId = null;
    private MeshRenderer child;
    private bool isInteracted = false;

    private TutoLearnWasteInteraction tutoLearnWasteInteraction;
    private PinApparition pinApparition;
    private void Awake()
    {
        id = Helpers.generateId();
        child = GetComponentInChildren<MeshRenderer>();
        tutoLearnWasteInteraction = FindAnyObjectByType<TutoLearnWasteInteraction>();
        pinApparition = GetComponent<PinApparition>();
    }

    void Update()
    {
        if (pinApparition)
        {
            bool alreadySet = false;
            PlayerInteraction[] players = FindObjectsOfType<PlayerInteraction>();
            players.Reverse();
            foreach (PlayerInteraction p in players)
            {
                if (!alreadySet)
                {
                    if (p.objectId == id)
                    {
                        alreadySet = true;
                        pinApparition.Appear();
                    }
                    else
                    {
                        pinApparition.Disappear();
                    }
                }
            }
        }
    }

    public void Interact(SystemId id)
    {
        if (SystemId.Pneu == typeId)
        {
            Debug.Log("Interacted with pneu");
        }

        if (!isInteracted)
        {
            isInteracted = true;
            hitPs.Play();
            if (hitSound)
                hitSound.Play();
            Destroy(child.gameObject);
            Invoke("Destroy", .5f);
            if (pinApparition) pinApparition.DestroyPin();
            if (tutoLearnWasteInteraction) tutoLearnWasteInteraction.onWasteDestroyByPlayer(typeId);
        }
    }

    void Destroy()
    {
        if (SystemId.Pneu == typeId)
        {
            Debug.Log("Destroy pneu");
        }
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

    public string getId()
    {
        return id;
    }
}