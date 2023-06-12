using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Stun params")]
    [SerializeField] public float stunTime = 1f;
    [SerializeField] public string stunTriggerName = "Wave";

    [Header("Reference")]
    [SerializeField] public ParticleSystem stunParticle;

    private bool isStun = false;
    private Animator animator = null;
    private PlayerMovement playerMovement;
    private SimpleSampleCharacterControl simpleSampleCharacter;
    private PlayerInteraction playerInteraction;


    private void Awake()
    {
        animator = gameObject.transform.parent.gameObject.GetComponent<Animator>();
        playerMovement = gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>();
        simpleSampleCharacter = gameObject.transform.parent.gameObject.GetComponent<SimpleSampleCharacterControl>();
        playerInteraction = gameObject.transform.parent.gameObject.GetComponent<PlayerInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waste")
        {
            StopCoroutine("Stun");
            StartCoroutine("Stun");
        }
    }

    IEnumerator Stun()
    {
        isStun = true;
        animator.SetTrigger(stunTriggerName);

        if (GameManager.Instance.debugMode)
            simpleSampleCharacter.enabled = false;
        else
            playerMovement.enabled = false;
        playerInteraction.enabled = false;

        stunParticle.Play();


        yield return new WaitForSeconds(stunTime);

        stunParticle.Stop();
        isStun = false;

        if (GameManager.Instance.debugMode)
            simpleSampleCharacter.enabled = true;
        else
            playerMovement.enabled = true;
        playerInteraction.enabled = true;

    }
}
