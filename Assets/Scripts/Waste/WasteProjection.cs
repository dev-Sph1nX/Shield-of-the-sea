using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteProjection : MonoBehaviour
{
    [SerializeField] public float gravity = 9.8f;
    [SerializeField] float height;
    [SerializeField] float timeToTravel;
    [SerializeField] float minPower, maxPower;
    [SerializeField] float minAnglePower, maxAnglePower;


    float objectT = 0;
    Vector3 a, b;
    bool isMoving = false;

    // public void LaunchObject(float rFactor)
    // {
    //     Rigidbody rb = GetComponent<Rigidbody>();

    //     float calcMaxPower = maxPower * (1 - Mathf.Abs(rFactor)) + minPower * Mathf.Abs(rFactor);
    //     float forwardPower = Random.Range(minPower, calcMaxPower);

    //     float calcMinSidePower = minAnglePower * (1 - Mathf.Abs(rFactor));
    //     float sidePower = Random.Range(calcMinSidePower, maxAnglePower);

    //     Debug.Log("r : " + rFactor + " / fpower : " + forwardPower + " / spower :" + sidePower);

    //     Vector3 launchForce = Vector3.up * forwardPower + Vector3.left * forwardPower + Vector3.forward * sidePower;
    //     rb.AddForce(launchForce, ForceMode.VelocityChange);
    // }

    public void LaunchObject(Vector3 start, Vector3 end)
    {
        a = start;
        b = end;
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            objectT = Time.time % timeToTravel;
            transform.position = SampleParabola(a, b, height, objectT);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isMoving = false;
    }
    void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(a, b);
        // float count = 20;
        // Vector3 lastP = a;
        // for (float i = 0; i < count + 1; i++)
        // {
        //     Vector3 p = SampleParabola(a, b, height, i / count);
        //     Gizmos.color = i % 2 == 0 ? Color.blue : Color.green;
        //     Gizmos.DrawLine(lastP, p);
        //     lastP = p;
        // }
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += Mathf.Sin(t * Mathf.PI) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross(right, travelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += (Mathf.Sin(t * Mathf.PI) * height) * up.normalized;
            return result;
        }
    }




}
