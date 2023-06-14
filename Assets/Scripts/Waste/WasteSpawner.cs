using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WasteSpawner : MonoBehaviour
{
    [Header("Spawn Params")]
    [SerializeField] float timeBetweenFire = 0.1f;
    [SerializeField][Range(110, 165)] float minLaunchVelocity = 110f;
    [SerializeField][Range(110, 165)] float maxLaunchVelocity = 165f;
    [SerializeField][Range(0, 10)] float torquePower = 50f;

    [Header("Spawn Area")]
    [SerializeField] float minZ;
    [SerializeField] float maxZ;
    [SerializeField] float securityMargin = 0.5f;
    [Header("Spawn Pneu Rotation")]
    [SerializeField] Quaternion pneuRotation = new Quaternion(90, 0, 0, 0);


    [Header("Reference")]
    [SerializeField] GameObject[] wastes;
    [SerializeField] GameObject pneu;

    private bool isPlaying = false, pneuIsAppear = false;
    private float nextFire = 0.0f;
    private Vector3 torque;
    private float timerPneuApparition;


    void Update()
    {
        if (Time.time > nextFire && isPlaying)
        {
            nextFire = Time.time + timeBetweenFire;

            GameObject model = wastes[Random.Range(0, wastes.Length)];
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, Random.Range(minZ + securityMargin, maxZ - securityMargin));
            float launchVelocity = Random.Range(minLaunchVelocity, maxLaunchVelocity);

            GameObject waste = Instantiate(model, spawnPosition, Quaternion.identity);

            torque.x = Random.Range(-torquePower, torquePower);
            torque.y = Random.Range(-torquePower, torquePower);
            torque.z = Random.Range(-torquePower, torquePower);
            waste.GetComponentInChildren<Rigidbody>().AddRelativeTorque(torque);
            waste.GetComponentInChildren<Rigidbody>().AddRelativeForce(new Vector3(-launchVelocity, launchVelocity * 2, 0));
        }
        if (Time.time > timerPneuApparition && !pneuIsAppear && isPlaying)
        {
            pneuIsAppear = true;
            Vector3 spawnPosition = transform.position + Vector3.forward * Random.Range(minZ + securityMargin, maxZ - securityMargin) + Vector3.up * 2;
            GameObject waste = Instantiate(pneu, spawnPosition, pneuRotation);
        }
        if (InputSystem.getButton("Fire1"))
        {
            pneuIsAppear = true;
            Vector3 spawnPosition = transform.position + Vector3.forward * Random.Range(minZ + securityMargin, maxZ - securityMargin) + Vector3.up * 2;
            GameObject waste = Instantiate(pneu, spawnPosition, pneuRotation);
        }
        // should appear at one time random 
    }

    public void StartGame(float gameTime)
    {
        timerPneuApparition = Random.Range(10f, gameTime - 10f); // bc not at the end
        isPlaying = true;
    }

    public void StopGame()
    {
        isPlaying = false;
    }

}
