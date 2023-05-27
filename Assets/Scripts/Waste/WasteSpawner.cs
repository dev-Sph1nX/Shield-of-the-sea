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
    [SerializeField] float intensity;
    [SerializeField] float power;
    [SerializeField] MinMax horizontalRange;


    [Header("Reference")]
    [SerializeField] GameObject cannette;
    [SerializeField] GameObject glassBottle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
