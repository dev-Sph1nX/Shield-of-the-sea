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
    GameObject player1;
    PlayerInteraction player1Interact;
    GameObject player2;
    PlayerInteraction player2Interact;
    LevelManager levelManager;
    private void Awake()
    {
        id = Helpers.generateId();
        child = GetComponentInChildren<MeshRenderer>();
        tutoLearnWasteInteraction = FindAnyObjectByType<TutoLearnWasteInteraction>();
        levelManager = FindAnyObjectByType<LevelManager>();
        pinApparition = GetComponent<PinApparition>();

        player1 = GameObject.FindWithTag("Player1");
        // if (player1 != null)
        // {
        //     player1Interact = player1.GetComponent<PlayerInteraction>();
        // }

        player2 = GameObject.FindWithTag("Player2");
        // if (player2 != null)
        // {
        //     player2Interact = player2.GetComponent<PlayerInteraction>();
        // }
    }

    void Update()
    {
        if (pinApparition && typeId != SystemId.Boss)
        {
            if (isInRangeOfAction())
                pinApparition.Appear();
            else
                pinApparition.Disappear();
        }
    }

    public void Interact(SystemId id)
    {
        if (SystemId.Pneu == typeId)
        {
            Debug.Log("Interacted with pneu");
            if (levelManager) levelManager.onPneuHit();
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


    bool isInRangeOfAction()
    {
        if (typeId == SystemId.Cannette)
        {
            if (CheckRange(transform.position.x, player1.transform.position.x - 2, player1.transform.position.x + 2) && CheckRange(transform.position.z, player1.transform.position.z - 2, player1.transform.position.z + 2))
            {
                return true;
            }

        }
        if (typeId == SystemId.Glass)
        {
            if (CheckRange(transform.position.x, player2.transform.position.x - 2, player2.transform.position.x + 2) && CheckRange(transform.position.z, player2.transform.position.z - 2, player2.transform.position.z + 2))
            {
                return true;
            }
        }
        return false;
    }

    static public bool CheckRange(float num, float min, float max)
    {
        return num > min && num < max;
    }
}