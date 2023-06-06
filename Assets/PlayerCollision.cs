using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Stun params")]
    [SerializeField] public float stunTime = 1f;
    [SerializeField] public string stunTriggerName = "Wave";

    [Header("Reference")]
    [SerializeField] public GameObject indicator;

    private bool isStun = false;
    private Animator animator = null;
    private Renderer indicatorRenderer = null;
    private Color initialColor;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        animator = gameObject.transform.parent.gameObject.GetComponent<Animator>();
        indicatorRenderer = indicator.GetComponent<Renderer>();
        initialColor = indicatorRenderer.material.color;
        playerMovement = gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>();
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

        playerMovement.enabled = false;
        indicatorRenderer.enabled = true;
        indicatorRenderer.material.color = new Color(0, 0, 0);
        print(gameObject.name + "is stun");

        yield return new WaitForSeconds(stunTime);

        print(gameObject.name + "is not more stun");
        isStun = false;
        playerMovement.enabled = true;
        indicatorRenderer.enabled = false;
        indicatorRenderer.material.color = initialColor;
    }
}
