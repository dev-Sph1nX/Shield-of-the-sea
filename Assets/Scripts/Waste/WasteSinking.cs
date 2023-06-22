using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteSinking : MonoBehaviour
{
    [Header("After ground collision")]
    [SerializeField] float mass = 1e-07f;
    [SerializeField] public float drag = 40;

    [Header("Under ground")]
    [SerializeField] float yLimitBeforeLoseIt = -0.1554092f;

    [Header("Reference")]
    [SerializeField] Collider colliderObj;
    [SerializeField] private ParticleSystem deathPs;
    [SerializeField] private ParticleSystem groundPs;
    [SerializeField] private PinApparition pinApparition;

    private LevelManager lvlManager;
    private TutoLearnWasteInteraction tutoLearnWasteInteraction;

    private bool isLost = false;
    private float baseMass, baseDrag;
    private Rigidbody rb;
    private WasteShadow shadowManager;
    private NPCInteractable npcInteractable;
    private GameObject tutoTarget;

    void Awake()
    {
        lvlManager = FindAnyObjectByType<LevelManager>();
        tutoLearnWasteInteraction = FindAnyObjectByType<TutoLearnWasteInteraction>();
        rb = GetComponentInChildren<Rigidbody>();
        shadowManager = GetComponent<WasteShadow>();
        npcInteractable = GetComponent<NPCInteractable>();

        baseMass = rb.mass;
        baseDrag = rb.drag;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "SinkingGround")
        {
            shadowManager.DestroyShadow();
            colliderObj.isTrigger = true;
            rb.mass = mass;
            rb.drag = drag;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            groundPs.Play();
            pinApparition.Appear();
            if (tutoLearnWasteInteraction)
            {
                tutoLearnWasteInteraction.onWasteFall();
                GameObject model = npcInteractable.typeId == SystemId.Cannette ? tutoLearnWasteInteraction.targetRedModel : tutoLearnWasteInteraction.targetBlueModel;
                if (model) // can be delete to avoid target
                    tutoTarget = Instantiate(model, transform.position, Quaternion.identity);
            }
        }
    }
    void Update()
    {
        if (transform.position.y < yLimitBeforeLoseIt && !isLost)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.position = new Vector3(transform.position.x, -0.4f, transform.position.z);

            isLost = true;
            deathPs.Play();
            if (lvlManager) lvlManager.OnWasteLost();
            pinApparition.Disappear();
            Invoke("Destroy", 1f);
        }
    }

    void Destroy()
    {
        if (tutoLearnWasteInteraction) tutoLearnWasteInteraction.onWasteLost();
        Destroy(gameObject);
    }
    public void DestroyTutoTarget()
    {
        Destroy(tutoTarget);
    }
}
