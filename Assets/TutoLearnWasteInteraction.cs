using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoLearnWasteInteraction : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject cannette;
    [SerializeField] GameObject glassBottle;
    [SerializeField] GameObject pneu;
    [SerializeField] GameObject spawn1;
    [SerializeField] GameObject spawn2;
    [SerializeField] GameObject spawnPneu;
    [SerializeField] float minLaunchVelocity;
    [SerializeField] float maxLaunchVelocity;
    [SerializeField] float tutoDrag;
    [SerializeField] float tutoPneuVelocity = 1;
    [SerializeField] public GameObject targetRedModel; // for movement target
    [SerializeField] public GameObject targetBlueModel; // for movement target

    [SerializeField] Quaternion pneuRotation = new Quaternion(90, 0, 0, 0);
    private Vector3 torque;
    private DialogManager dialogManager;
    private bool waitingBothDestroy = false; // waitingP1Destroy = false, waitingP2Destroy = false
    private bool waitingPlayersProximity = false, p1isIn = false, p2isIn = false;
    private bool p1hasInteract = false, p2hasInteract = false;
    private bool p1trigger = false, p2trigger = false;
    private bool needFall = false;
    private bool waitingAllDestroy = false, cannetteTrigger = false, glassTrigger = false, pneuTrigger = false;
    void Start()
    {
        dialogManager = FindAnyObjectByType<DialogManager>();
    }

    void Update()
    {
        // if (InputSystem.getButton("Fire1")) throwTwoWaste();
        if (waitingPlayersProximity && p1isIn && p2isIn)
        {
            // these 2 in ! -> so let's reset
            p1isIn = false;
            p2isIn = false;
            waitingPlayersProximity = false;
            clearTarget();
            dialogManager.OnNextStep();
        }
    }

    public void throwWaste(GameObject model, Vector3 position, float launchVelocity, bool overrideDrag = false)
    {
        GameObject waste = Instantiate(model, position, Quaternion.identity);
        torque.x = Random.Range(-5, 5);
        torque.y = Random.Range(-5, 5);
        torque.z = Random.Range(-5, 5);
        if (overrideDrag) waste.GetComponent<WasteSinking>().drag = tutoDrag;
        waste.GetComponentInChildren<Rigidbody>().AddRelativeTorque(torque);
        waste.GetComponentInChildren<Rigidbody>().AddRelativeForce(new Vector3(-launchVelocity, launchVelocity * 2, 0));
    }
    public void throwTwoWaste()
    {
        throwWaste(cannette, spawn1.transform.position, maxLaunchVelocity, true);
        throwWaste(glassBottle, spawn2.transform.position, minLaunchVelocity, true);
        needFall = true;
    }

    public void onWasteFall()
    {
        if (needFall)
        {
            needFall = false;
            dialogManager.OnNextStep();
        }
    }

    public void onWasteLost()
    {
        // if (waitingPlayersProximity || waitingAllDestroy || waitingBothDestroy)
        // {
        dialogManager.RestartStep();
        // Debug.Log("Loser");
        // }
    }
    public void onWasteDestroyByPlayer(SystemId typeId)
    {
        if (typeId == SystemId.Cannette)
        {
            p1trigger = true;
            cannetteTrigger = true;
        }

        if (typeId == SystemId.Glass)
        {
            p2trigger = true;
            glassTrigger = true;
        }
        if (typeId == SystemId.Pneu)
        {
            pneuTrigger = true;
        }

        if (waitingBothDestroy)
        {
            if (p1trigger)
            {
                p1trigger = false;
                p1hasInteract = true;
            }

            if (p2trigger)
            {
                p2trigger = false;
                p2hasInteract = true;
            }

            if (p1hasInteract && p2hasInteract)
            {
                dialogManager.OnNextStep();
                waitingBothDestroy = false;
            }
        }
        if (waitingAllDestroy && cannetteTrigger && glassTrigger && pneuTrigger)
        {
            dialogManager.OnNextStep();
        }
    }

    public void waitingProximity()
    {
        waitingPlayersProximity = true;
    }


    public void onP1Collide()
    {
        p1isIn = !p1isIn;
    }
    public void onP2Collide()
    {
        p2isIn = !p2isIn;
    }

    public void waitingInteraction()
    {
        waitingBothDestroy = true;
    }

    void clearTarget()
    {
        WasteSinking[] wasteSinkings = FindObjectsOfType<WasteSinking>();
        foreach (WasteSinking wasteSinking in wasteSinkings)
        {
            wasteSinking.DestroyTutoTarget();
        }
    }

    public void throwSecondWave()
    {
        targetRedModel = null;
        targetBlueModel = null;
        throwWaste(glassBottle, spawn1.transform.position, minLaunchVelocity);
        throwWaste(cannette, spawn2.transform.position, maxLaunchVelocity);
        throwPneu();
        waitingAllDestroy = true;
    }

    public void throwPneu()
    {
        GameObject waste = Instantiate(pneu, spawnPneu.transform.position, pneuRotation);
        waste.GetComponent<PneuMovement>().applyXVelocity = tutoPneuVelocity;
    }
}
