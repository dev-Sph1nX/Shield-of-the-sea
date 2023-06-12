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
    [SerializeField] GameObject shadow;

    private Vector3 position;
    private float actualSize;
    // Start is called before the first frame update
    private bool isDestroy = false;
    void Start()
    {
        actualSize = maxSize;
        position = new Vector3(transform.position.x, y, transform.position.z);
        NPCInteractable interactable = GetComponent<NPCInteractable>();
        shadow = Instantiate(shadow, position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDestroy)
        {
            float newSize = CalculateShadowScale();
            UpdateSize(newSize);
            shadow.transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }

    float CalculateShadowScale()
    {
        float percentage = transform.position.y / topHeight;
        return maxSize * OutExpo(percentage);
    }

    void UpdateSize(float size)
    {
        // actualSize = Mathf.Lerp(size, actualSize, Time.deltaTime);
        shadow.transform.localScale = new Vector3(size, 0.01f, size);
    }

    public static float InExpo(float t) => (float)Mathf.Pow(2, 10 * (t - 1));
    public static float OutExpo(float t) => 1 - InExpo(1 - t);

    public void DestroyShadow()
    {
        isDestroy = true;
        Destroy(shadow);
    }
}
