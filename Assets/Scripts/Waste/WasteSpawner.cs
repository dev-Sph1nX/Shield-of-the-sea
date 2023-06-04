using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WasteSpawner : MonoBehaviour
{
    [Header("Spawn Params")]
    [SerializeField] float timeBetweenFire = 0.1f;
    [SerializeField][Range(250, 500)] float minLaunchVelocity = 250f;
    [SerializeField][Range(250, 500)] float maxLaunchVelocity = 500f;

    [Header("Spawn Area")]
    [SerializeField] float minZ;
    [SerializeField] float maxZ;
    [SerializeField] float securityMargin = 0.5f;

    [Header("Reference")]
    [SerializeField] GameObject[] wastes;

    private bool isPlaying = false;
    private float nextFire = 0.0f;

    void Update()
    {
        if (Time.time > nextFire && isPlaying)
        {
            nextFire = Time.time + timeBetweenFire;

            GameObject model = wastes[Random.Range(0, wastes.Length)];
            Vector3 spawnPosition = transform.position + Vector3.forward * Random.Range(minZ + securityMargin, maxZ - securityMargin);
            float launchVelocity = Random.Range(minLaunchVelocity, maxLaunchVelocity);

            GameObject waste = Instantiate(model, spawnPosition, transform.rotation);
            waste.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-launchVelocity, launchVelocity, 0));
        }
    }

    public void StartGame()
    {
        isPlaying = true;
    }

}
