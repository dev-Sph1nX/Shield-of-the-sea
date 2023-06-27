using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinApparition : MonoBehaviour
{
    [SerializeField] GameObject model;
    [SerializeField] float height;
    [SerializeField] float x = 0;

    private GameObject pin;
    bool destroy = false;

    void Start()
    {
        pin = Instantiate(model, gameObject.transform.position + Vector3.up * height + Vector3.right * x, Quaternion.identity);
        Disappear(); // set false by default
    }

    void Update()
    {
        if (!destroy)
            pin.transform.position = gameObject.transform.position + Vector3.up * height + Vector3.right * x;
    }

    public void Appear()
    {
        if (!destroy)
        {
            if (!pin.activeInHierarchy)
            {
                pin.SetActive(true);
            }
        }
    }

    public void Disappear()
    {
        if (!destroy)
        {
            if (pin.activeInHierarchy)
            {
                pin.SetActive(false);
            }
        }
    }
    public void DestroyPin()
    {
        destroy = true;
        if (pin)
        {
            Destroy(pin);
        }
    }
}
