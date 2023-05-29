using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WasteSpawner : MonoBehaviour
{
    [Header("Spawn Params")]
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float minX, maxX;
    [SerializeField] float minZ, maxZ;

    [Header("Reference")]
    [SerializeField] GameObject[] wastes;

    private float nextFire = 0.0f;


    void Update()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject waste = Instantiate(wastes[Random.Range(0, wastes.Length - 1)], transform.position, Quaternion.identity);
            Vector3 end = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
            waste.GetComponent<WasteProjection>().LaunchObject(transform.position, end);

        }
    }

}
