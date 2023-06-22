using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [Range(1, 6)][SerializeField] float speed = 2;
    [Range(0, 2)][SerializeField] float amplificateur = 2;

    void Update()
    {
        Vector3 position = transform.localPosition;
        position.y = Mathf.Sin(Time.time * speed) * amplificateur;
        transform.localPosition = position;
    }

}
