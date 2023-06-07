using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteShadow : MonoBehaviour
{

    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float y = 0.01f;
    [SerializeField] float maxSize = 2f;
    [SerializeField] float topHeight = 15f;

    [Header("Reference")]
    [SerializeField] GameObject shadowPrefab;

    private GameObject shadow;
    private Vector3 position;
    private float actualSize;
    // Start is called before the first frame update
    void Start()
    {
        actualSize = maxSize;
        position = new Vector3(transform.position.x, y, transform.position.z);
        shadow = Instantiate(shadowPrefab, position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        float newSize = CalculateShadowScale();
        UpdateSize(newSize);

        // position = Vector3.Lerp(position, new Vector3(transform.position.x, y, transform.position.z), Time.deltaTime);
        shadow.transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    float CalculateShadowScale()
    {
        // Ray ray = new Ray(transform.position, transform.forward);
        // Debug.DrawRay(transform.position, Vector3.down * 30);
        // RaycastHit hitData;
        // if (Physics.Raycast(ray, out hitData, 30, groundLayerMask))
        // {
        //     // Debug.Log(hitData.distance);
        //     // have to link hitData.distance and size

        // }

        float percentage = transform.position.y / topHeight;
        return maxSize * percentage;
    }

    void UpdateSize(float size)
    {
        // actualSize = Mathf.Lerp(size, actualSize, Time.deltaTime);
        shadow.transform.localScale = new Vector3(size, 0.01f, size);
    }
}
