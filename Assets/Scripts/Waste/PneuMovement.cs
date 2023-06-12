using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PneuMovement : MonoBehaviour
{
    [Header("Forward force")]
    [SerializeField] float applyXVelocity = 1f;

    [Header("X Rotation")]
    [Range(1, 6)][SerializeField] float amplitude = 1f;
    [Range(1, 1000)][SerializeField] float frequency = 1f;
    [Header("Death")]
    [SerializeField] float animationTime = 1f;
    [SerializeField] ParticleSystem deathParticule;

    Rigidbody rb;
    private Quaternion initialRotation;
    private bool isLost = false;
    private LevelManager lvlManager;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
        lvlManager = FindAnyObjectByType<LevelManager>();
    }

    void Update()
    {
        Vector2 vel = rb.velocity;
        vel.x = -applyXVelocity;
        rb.velocity = vel;

        float rotationAngle = amplitude * Mathf.Sin(2f * Mathf.PI * frequency * Time.time);

        Quaternion rotation = initialRotation * Quaternion.Euler(rotationAngle, 0f, 0f);
        transform.rotation = rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PneuCollider" && !isLost)
        {
            isLost = true;
            LostInTheSea();
        }
    }

    void LostInTheSea()
    {
        deathParticule.Play();
        lvlManager.OnWasteLost();
        Invoke("Destroy", animationTime);
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
