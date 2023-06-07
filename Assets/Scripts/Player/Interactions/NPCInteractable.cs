using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [Header("Type")]
    [SerializeField] public SystemId typeId;

    [Header("Id")]
    [SerializeField][ReadOnly] string _id;
    private SystemId? ownerId = null;
    private WasteSinking wasteSinking;
    private ParticleSystem particle;
    private MeshRenderer child;
    private bool isInteracted = false;

    private void Awake()
    {
        _id = Helpers.generateId();
        wasteSinking = GetComponent<WasteSinking>();
        particle = GetComponentInChildren<ParticleSystem>();
        child = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        if (ownerId != null)
        {
            PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
            foreach (PlayerMovement p in players)
            {
                if (p.playerId == ownerId)
                {
                    transform.position = p.transform.position + Vector3.up * 2;
                }
            }
        }
    }

    public void Interact(SystemId id)
    {
        if (!isInteracted)
        {
            isInteracted = true;
            particle.Play();
            Destroy(child.gameObject);
            Invoke("Destroy", .5f);
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

    public string GetId()
    {
        return _id;
    }

    public bool isInteractable()
    {
        return !isInteracted;
    }
}