using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField][ReadOnly] string _id;
    private PlayerId? ownerId = null;
    private WasteSinking wasteSinking;

    private void Awake()
    {
        _id = Helpers.generateId();
        wasteSinking = GetComponent<WasteSinking>();
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

    public void Interact(PlayerId id)
    {
        if (ownerId == null)
        {
            Debug.Log("Pick up !");
            ownerId = id;
        }
        else
        {
            Debug.Log("Release !");
            ownerId = null;
            wasteSinking.gotRelease();
        }
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
        return ownerId != null ? IconType.Release : IconType.Take;
    }

    public string GetId()
    {
        return _id;
    }

}