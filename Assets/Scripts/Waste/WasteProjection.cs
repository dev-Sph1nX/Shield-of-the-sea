using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteProjection : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public float power = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(Vector3.back * power);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject getGameObject()
    {
        return gameObject;
    }
}
