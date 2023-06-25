using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] GameObject leftFootGo;
    [SerializeField] GameObject rightFootGo;
    [SerializeField] float yLimit;
    [SerializeField] float startMovingLimit = .2f;
    [SerializeField] AudioSource run;

    Vector3 leftFoot, rightFoot;
    PlayerMovement playerMovement;
    SimpleSampleCharacterControl simpleSampleCharacter;
    bool isMoving = false;
    float localVelocity;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        simpleSampleCharacter = GetComponent<SimpleSampleCharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.debugMode)
            localVelocity = simpleSampleCharacter.velocity;
        else
            localVelocity = playerMovement.velocity;

        if (localVelocity > startMovingLimit)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        leftFoot = leftFootGo.transform.TransformPoint(Vector3.zero);
        rightFoot = rightFootGo.transform.TransformPoint(Vector3.zero);

        if (isMoving)
        {
            if (leftFoot.y < yLimit || rightFoot.y < yLimit)
            {
                if (!run.isPlaying)
                {
                    run.Play();
                }
            }
        }
    }
}
