using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteProjection : MonoBehaviour
{
    [SerializeField] float height; //desired parabola height
    [SerializeField] float timeToTravel; //desired time to complete parabola
    float objectT = 0; //timer for that object
    Vector3 a, b; //Vector positions for start and end
    bool isMoving = false;

    public void StartParabolicMovement(Vector3 start, Vector3 end, int id)
    {
        gameObject.name = "cannette" + id;
        a = start;
        b = end;
        isMoving = true;
        Debug.Log(gameObject.name + " start : " + start.ToString() + " // end : " + end.ToString());
        Debug.Log(gameObject.name + " is at " + transform.position);
    }


    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //Shows how to animate something following a parabola
            objectT = Time.time % timeToTravel;
            transform.position = SampleParabola(a, b, height, objectT);
            // if (objectT > 1)
            // {
            //     isMoving = false;
            // }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isMoving = false;
    }
    void OnDrawGizmos()
    {
        //Draw the height in the viewport, so i can make a better gif :]
        // Handles.BeginGUI();
        // GUI.skin.box.fontSize = 16;
        // GUI.Box(new Rect(10, 10, 100, 25), h + "");
        // Handles.EndGUI();

        //Draw the parabola by sample a few times
        Gizmos.color = Color.red;
        Gizmos.DrawLine(a, b);
        float count = 20;
        Vector3 lastP = a;
        for (float i = 0; i < count + 1; i++)
        {
            Vector3 p = SampleParabola(a, b, height, i / count);
            Gizmos.color = i % 2 == 0 ? Color.blue : Color.green;
            Gizmos.DrawLine(lastP, p);
            lastP = p;
        }
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
