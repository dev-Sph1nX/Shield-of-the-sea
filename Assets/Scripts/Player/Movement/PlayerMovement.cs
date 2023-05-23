using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Initial Position")]
    [SerializeField] float positionX;
    [SerializeField] float positionY;
    [SerializeField] float positionZ;


    [Header("Movement Area")]
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;
    [Header("Movement settings")]
    [SerializeField][Range(1, 20)] float expo = 10;
    [SerializeField][Range(0, 0.2f)] float mouvementSensibility;

    [Header("Reference")]
    [SerializeField] private Animator m_animator = null;


    private readonly float m_interpolation = 10;
    private Vector3 localPosition, direction;
    private Quaternion localRotation, tempRotation;
    private float deltaTime, velocity;
    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();

    private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        localPosition = new Vector3(positionX, positionY, positionZ);
        localRotation = new Quaternion(0, 0, 0, 0);
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

        m_animator.SetBool("Grounded", m_isGrounded);
        PlayerUpdate();
    }

    public void sendData(Coord coord)
    {
        // Récupération des coordonnées
        Vector2 percentage = new Vector2((coord.x / 100), (coord.y / 100));

        // Calcul des positions à partir de pourcentage
        float newPositionX = calcPosition(maxX, minX, percentage.x);
        float newPositionZ = calcPosition(maxZ, minZ, percentage.y);

        direction = new Vector3(newPositionX, localPosition.y, newPositionZ) - localPosition;
        Debug.DrawRay(localPosition, direction, Color.green, 1);

        if (direction.magnitude > mouvementSensibility)
        {
            // Position
            positionX = newPositionX;
            positionZ = newPositionZ;
            localPosition.x = Mathf.Lerp(localPosition.x, positionX, deltaTime * m_interpolation);
            localPosition.z = Mathf.Lerp(localPosition.z, positionZ, deltaTime * m_interpolation);

            // Direction
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.down);
            Quaternion newRotation = Quaternion.Slerp(localRotation, angleAxis, deltaTime * m_interpolation);
            localRotation = Quaternion.Lerp(newRotation, localRotation, deltaTime * m_interpolation);
        }
    }
    private void PlayerUpdate()
    {
        direction = Vector3.Lerp(direction, Vector3.zero, Time.deltaTime * m_interpolation);
        float magnitude = direction.magnitude * expo;
        if (direction.magnitude < mouvementSensibility)
        {
            magnitude = 0;
        }
        velocity = Mathf.Lerp(velocity, magnitude, Time.deltaTime * m_interpolation);
        m_animator.SetFloat("MoveSpeed", velocity);

    }
    private float calcPosition(float max, float min, float percentage)
    {
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
                m_isGrounded = true;
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
            m_isGrounded = true;
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
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

}