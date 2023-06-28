using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerId")]
    [SerializeField] public SystemId playerId;

    [Header("Movement Area")]
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;

    [Header("Movement settings")]
    [SerializeField][Range(1, 20)] float interpolation = 10;
    [SerializeField][Range(0, 1f)] float mouvementSensibility;
    [SerializeField] bool inverseZ;
    [SerializeField] bool inverseX;
    [SerializeField] float debugIncrement = 1f;

    [Header("Reference")]
    [SerializeField] private Animator m_animator = null;


    private float positionX, positionY, positionZ;
    private Vector3 localPosition, direction;
    private Quaternion localRotation, tempRotation;
    private float deltaTime;
    public float velocity;
    private List<Collider> m_collisions = new List<Collider>();
    private int layerBoss;
    private bool isStopped = false;
    private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        localPosition = transform.position;

        positionX = localPosition.x;
        positionZ = localPosition.z;

        localRotation = new Quaternion(0, 0, 0, 0);
        // if (GameManager.Instance.debugMode)
        // {
        //     GetComponent<SimpleSampleCharacterControl>().enabled = true;
        //     this.enabled = false;
        // }
        layerBoss = LayerMask.GetMask("Boss");
    }

    private void Update()
    {
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        deltaTime = Time.deltaTime;
    }

    void FixedUpdate()
    {
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        deltaTime = Time.deltaTime;

        PlayerUpdate();

        if (GameManager.Instance.debugMode)
        {
            DebugPlayerUpdate();
        }
    }

    public void sendData(Coord coord)
    {
        if (isStopped)
        {
            return;
        }
        // Récupération des coordonnées
        Vector2 percentage = new Vector2((coord.x / 100), (coord.y / 100));

        // Calcul des positions à partir de pourcentage
        float newPositionX = calcPosition(maxX, minX, percentage.x, inverseX);
        float newPositionZ = calcPosition(maxZ, minZ, percentage.y, inverseZ);

        direction = new Vector3(newPositionX, localPosition.y, newPositionZ) - localPosition;
        Debug.DrawRay(localPosition, direction, Color.green, 1);

        if (direction.magnitude > mouvementSensibility)
        {
            // Position
            positionX = newPositionX;
            positionZ = newPositionZ;
        }
    }

    private void PlayerUpdate()
    {
        direction = Vector3.Lerp(direction, Vector3.zero, Time.deltaTime * interpolation);
        float magnitude = direction.magnitude * 5;
        if (magnitude > mouvementSensibility) // direction.magnitude
        {
            // Suite Position
            localPosition.x = Mathf.Lerp(localPosition.x, positionX, deltaTime * interpolation);
            localPosition.z = Mathf.Lerp(localPosition.z, positionZ, deltaTime * interpolation);


            // Direction
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.down);
            Quaternion newRotation = Quaternion.Slerp(localRotation, angleAxis, Time.deltaTime * interpolation);
            localRotation = Quaternion.Lerp(newRotation, localRotation, Time.deltaTime * interpolation);
        }
        else
        {
            magnitude = 0;
        }

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(localPosition.x, localPosition.y + 100f, localPosition.z), Vector3.down, out hit, Mathf.Infinity, layerBoss))
        {
            float terrainY = RoundValue(hit.point.y, 100);
            if (terrainY <= 0)
            {
                terrainY = 0;
            }
            localPosition.y = terrainY;
        }

        velocity = Mathf.Lerp(velocity, magnitude, Time.deltaTime * interpolation);
        m_animator.SetFloat("MoveSpeed", velocity);

    }

    private void DebugPlayerUpdate()
    {
        if (isStopped)
        {
            return;
        }

        float v = Input.GetAxis(playerId == SystemId.Player1 ? "DP1 - Vertical" : "DP2 - Vertical");
        float h = Input.GetAxis(playerId == SystemId.Player1 ? "DP1 - Horizontal" : "DP2 - Horizontal");

        float newPositionZ = localPosition.z + debugIncrement * v;
        float newPositionX = localPosition.x + debugIncrement * h;


        direction = new Vector3(newPositionX, localPosition.y, newPositionZ) - localPosition;
        Debug.DrawRay(localPosition, direction, Color.green, 1);

        if (direction.magnitude > mouvementSensibility)
        {
            // Position
            positionX = newPositionX;
            positionZ = newPositionZ;
        }
    }

    // private void debugPlayerMovement(PlayerId id)
    // {
    //     float h = InputSystem.getHorizontalAxis(id);
    //     float v = InputSystem.getVerticalAxis(id);
    //     float newPositionZ = localPosition.z += v * 0.1f;
    //     float newPositionX = localPosition.x += h * 0.1f;
    //     direction = new Vector3(newPositionX, localPosition.y, newPositionZ) - localPosition;
    //     positionX = newPositionX;
    //     positionZ = newPositionZ;
    // }

    private float calcPosition(float max, float min, float percentage, bool isInversed)
    {
        if (isInversed)
        {
            float temp = max;
            max = min;
            min = temp;
        }
        return (max - min) * percentage + min;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
    }

    float RoundValue(float num, float precision)
    {
        return Mathf.Floor(num * precision + 0.5f) / precision;
    }

    public void GetStop()
    {
        isStopped = true;
    }
}
