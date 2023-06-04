using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteSinking : MonoBehaviour
{
    [Header("After ground collision")]
    [SerializeField] float mass = 1e-07f;
    [SerializeField] float drag = 40;

    [Header("Under ground")]
    [SerializeField] float yLimitBeforeLoseIt = -0.1554092f;
    [Header("Throw")]
    [SerializeField] float throwPower = 100f;

    [Header("Reference")]
    [SerializeField] Collider colliderObj;

    private LevelManager lvlManager;

    private bool isLost = false;
    private float baseMass, baseDrag;
    private Rigidbody rb;

    void Awake()
    {
        lvlManager = FindAnyObjectByType<LevelManager>();
        rb = GetComponent<Rigidbody>();

        baseMass = rb.mass;
        baseDrag = rb.drag;
    }

    public void gotThrown()
    {
        colliderObj.isTrigger = false;
        rb.mass = baseMass;
        rb.drag = baseDrag;
        rb.AddRelativeForce(new Vector3(-throwPower, throwPower, 0));
    }
    public void gotRelease()
    {
        colliderObj.isTrigger = false;
        rb.mass = baseMass;
        rb.drag = baseDrag;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "SinkingGround")
        {
            colliderObj.isTrigger = true;
            rb.mass = mass;
            rb.drag = drag;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    void Update()
    {
        if (transform.position.y < yLimitBeforeLoseIt && !isLost)
        {
            isLost = true;
            Destroy(gameObject);
            lvlManager.OnWasteLost();
        }
    }
}
