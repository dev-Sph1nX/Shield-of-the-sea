using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System.Linq;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [Header("Type")]
    [SerializeField] public SystemId typeId;
    [SerializeField] public int interactRange = 2;

    [Header("References")]

    [SerializeField] private ParticleSystem hitPs;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] public bool interactable = false;
    [SerializeField][ReadOnly] private string id;

    private SystemId? ownerId = null;
    private MeshRenderer child;

    private TutoLearnWasteInteraction tutoLearnWasteInteraction;
    private PinApparition pinApparition;
    PinPneuManager pinPneuManager;
    GameObject player1;
    PlayerInteraction player1Interact;
    GameObject player2;
    PlayerInteraction player2Interact;
    LevelManager levelManager;
    private WasteShadow shadowManager;
    private void Awake()
    {
        id = Helpers.generateId();
        child = GetComponentInChildren<MeshRenderer>();
        tutoLearnWasteInteraction = FindAnyObjectByType<TutoLearnWasteInteraction>();
        levelManager = FindAnyObjectByType<LevelManager>();
        pinApparition = GetComponent<PinApparition>();
        shadowManager = GetComponent<WasteShadow>();
        pinPneuManager = GetComponent<PinPneuManager>();
        player1 = GameObject.FindWithTag("Player1");
        player2 = GameObject.FindWithTag("Player2");
    }

    void Update()
    {
        if (interactable && pinApparition && typeId != SystemId.Boss && typeId != SystemId.Pneu) // bc boss and pneu pin's are manage by an other script
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
            if (levelManager) levelManager.onPneuHit();
        }

        if (interactable)
        {
            if (shadowManager)
                shadowManager.DestroyShadow();
            if (pinPneuManager)
                pinPneuManager.DestroyPin();
            interactable = false;
            hitPs.Play();
            if (hitSound)
                hitSound.Play();
            Destroy(child.gameObject);
            Invoke("Destroy", .5f);
            if (pinApparition) pinApparition.DestroyPin();
            if (tutoLearnWasteInteraction) tutoLearnWasteInteraction.onWasteDestroyByPlayer(typeId);
        }
    }

    public void TouchGround()
    {
        interactable = true;
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
        return interactable;
    }

    public string getId()
    {
        return id;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRange);

    }


    bool isInRangeOfAction()
    {
        if (typeId == SystemId.Cannette)
        {
            if (CheckRange(transform.position.x, player1.transform.position.x - interactRange, player1.transform.position.x + interactRange) && CheckRange(transform.position.z, player1.transform.position.z - interactRange, player1.transform.position.z + interactRange))
            {
                return true;
            }

        }
        if (typeId == SystemId.Glass)
        {
            if (CheckRange(transform.position.x, player2.transform.position.x - interactRange, player2.transform.position.x + interactRange) && CheckRange(transform.position.z, player2.transform.position.z - interactRange, player2.transform.position.z + interactRange))
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

    public int getRange()
    {
        return interactRange;
    }
}