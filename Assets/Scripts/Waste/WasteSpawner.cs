using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax
{
    public float min;
    public float max;
}

public class WasteSpawner : MonoBehaviour
{
    [Header("Spawn Params")]
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float minPower;
    [SerializeField] float maxPower;
    [SerializeField] MinMax horizontalRange;


    [Header("Reference")]
    [SerializeField] GameObject cannette;
    [SerializeField] GameObject glassBottle;
    private float nextFire = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            float power = Random.Range(minPower, maxPower);
            GameObject waste = Instantiate(cannette, transform.position, Quaternion.identity);
            waste.GetComponent<Rigidbody>().AddForce(Vector3.up * power + Vector3.left * power / 2);
        }
    }
}
