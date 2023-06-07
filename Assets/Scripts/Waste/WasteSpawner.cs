using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WasteSpawner : MonoBehaviour
{
    [Header("Spawn Params")]
    [SerializeField] float timeBetweenFire = 0.1f;
    [SerializeField][Range(100, 165)] float minLaunchVelocity = 250f;
    [SerializeField][Range(100, 165)] float maxLaunchVelocity = 500f;
    [SerializeField][Range(0, 10)] float torquePower = 50f;

    [Header("Spawn Area")]
    [SerializeField] float minZ;
    [SerializeField] float maxZ;
    [SerializeField] float securityMargin = 0.5f;

    [Header("Reference")]
    [SerializeField] GameObject[] wastes;

    private bool isPlaying = false;
    private float nextFire = 0.0f;
    private Vector3 torque;


    void Update()
    {
        if (Time.time > nextFire && isPlaying)
        {
            nextFire = Time.time + timeBetweenFire;

            GameObject model = wastes[Random.Range(0, wastes.Length)];
            Vector3 spawnPosition = transform.position + Vector3.forward * Random.Range(minZ + securityMargin, maxZ - securityMargin);
            float launchVelocity = Random.Range(minLaunchVelocity, maxLaunchVelocity);

            GameObject waste = Instantiate(model, spawnPosition, Quaternion.identity);

            torque.x = Random.Range(-torquePower, torquePower);
            torque.y = Random.Range(-torquePower, torquePower);
            torque.z = Random.Range(-torquePower, torquePower);
            waste.GetComponent<Rigidbody>().AddRelativeTorque(torque);
            waste.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-launchVelocity, launchVelocity * 2, 0));
        }
    }

    public void StartGame()
    {
        isPlaying = true;
    }

    public void StopGame()
    {
        isPlaying = false;
    }

}
