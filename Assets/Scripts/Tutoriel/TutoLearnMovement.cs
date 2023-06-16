using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutoLearnMovement : MonoBehaviour
{
    [Header("Target Entites")]
    [SerializeField] MeshRenderer topLeftTarget;
    [SerializeField] MeshRenderer topRightTarget;
    [SerializeField] MeshRenderer bottomLeftTarget;
    [SerializeField] MeshRenderer bottomRightTarget;
    [SerializeField] MeshRenderer middleLeftTarget;
    [SerializeField] MeshRenderer middleRightTarget;
    [Header("Target Material")]
    [SerializeField] Material redMaterial;
    [SerializeField] Material blueMaterial;
    private DialogManager dialogManager;
    private bool waitingPlayers = false, p1isIn = false, p2isIn = false;

    public void Start()
    {
        // dialogManager.OnNextStep quand l'action est fini
        dialogManager = FindAnyObjectByType<DialogManager>();
    }

    public void Update()
    {
        if (waitingPlayers && p1isIn && p2isIn)
        {
            // these 2 in ! -> so let's reset
            p1isIn = false;
            p2isIn = false;
            waitingPlayers = false;
            ResetTarget();
            dialogManager.OnNextStep();
        }
    }

    public void MovetoTarget1()
    {
        Debug.Log("LearnMovement");

        // afficher les cibles
        topLeftTarget.material = redMaterial;
        topLeftTarget.gameObject.SetActive(true);

        bottomRightTarget.material = blueMaterial;
        bottomRightTarget.gameObject.SetActive(true);

        waitingPlayers = true;
    }

    public void MovetoTarget2()
    {
        Debug.Log("LearnMovement part 2");

        // afficher les cibles
        topRightTarget.material = redMaterial;
        topRightTarget.gameObject.SetActive(true);

        bottomLeftTarget.material = blueMaterial;
        bottomLeftTarget.gameObject.SetActive(true);

        waitingPlayers = true;
    }

    public void MovetoTarget3()
    {
        Debug.Log("LearnMovement part 3");

        // afficher les cibles
        bottomRightTarget.material = redMaterial;
        bottomRightTarget.gameObject.SetActive(true);

        topLeftTarget.material = blueMaterial;
        topLeftTarget.gameObject.SetActive(true);

        waitingPlayers = true;
    }
    public void MovetoTarget4()
    {
        Debug.Log("LearnMovement part 4");

        // afficher les cibles
        bottomLeftTarget.material = redMaterial;
        bottomLeftTarget.gameObject.SetActive(true);

        topRightTarget.material = blueMaterial;
        topRightTarget.gameObject.SetActive(true);

        waitingPlayers = true;
    }
    public void MovetoTarget5()
    {
        Debug.Log("LearnMovement part 5");

        // afficher les cibles
        middleLeftTarget.material = redMaterial;
        middleLeftTarget.gameObject.SetActive(true);

        middleRightTarget.material = blueMaterial;
        middleRightTarget.gameObject.SetActive(true);

        waitingPlayers = true;
    }

    public void onP1Collide()
    {
        p1isIn = !p1isIn;
        Debug.Log("p1 in/out");
    }
    public void onP2Collide()
    {
        Debug.Log("p2 in/out");
        p2isIn = !p2isIn;
    }

    void ResetTarget()
    {
        topLeftTarget.gameObject.SetActive(false);
        topRightTarget.gameObject.SetActive(false);
        bottomRightTarget.gameObject.SetActive(false);
        bottomLeftTarget.gameObject.SetActive(false);
    }
}
