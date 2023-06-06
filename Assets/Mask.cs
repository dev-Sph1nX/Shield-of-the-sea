using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    [SerializeField][Range(0, 1)] public float _size;
    [Header("Reference")]
    [SerializeField] Material material;

    private float __sizeActualValue;

    private void Awake()
    {
        __sizeActualValue = material.GetFloat("_Size");
    }

    private void Update()
    {
        if (_size != __sizeActualValue)
        {
            __sizeActualValue = _size;
            material.SetFloat("_Size", _size);
        }
    }

}
