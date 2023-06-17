using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoLearnWasteInteraction : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject cannette;
    [SerializeField] GameObject glassBottle;
    [SerializeField] GameObject spawn1;
    [SerializeField] GameObject spawn2;
    [SerializeField] float launchVelocity;
    [SerializeField] float tutoDrag;


    private Vector3 torque;
    private DialogManager dialogManager;
    private bool waitingP1Destroy = false, waitingP2Destroy = false, waitingBothDestroy = false;
    private bool p1hasInteract = false, p2hasInteract = false;
    private bool p1trigger = false, p2trigger = false;
    void Start()
    {
        dialogManager = FindAnyObjectByType<DialogManager>();
    }

    // void Update()
    // {
    //     if (InputSystem.getButton("Fire1")) throwOneWaste();

    // }

    public void throwWaste(GameObject model, Vector3 position, bool overrideDrag = false)
    {
        GameObject waste = Instantiate(model, position, Quaternion.identity);
        torque.x = Random.Range(-5, 5);
        torque.y = Random.Range(-5, 5);
        torque.z = Random.Range(-5, 5);
        if (overrideDrag) waste.GetComponent<WasteSinking>().drag = tutoDrag;
        waste.GetComponentInChildren<Rigidbody>().AddRelativeTorque(torque);
        waste.GetComponentInChildren<Rigidbody>().AddRelativeForce(new Vector3(-launchVelocity, launchVelocity * 2, 0));
    }
    public void throwOneWaste()
    {
        throwWaste(cannette, spawn1.transform.position, true);
    }
    public void onWasteLost()
    {
        if (waitingP1Destroy || waitingP2Destroy || waitingBothDestroy)
        {
            Debug.Log("Loser");
        }
        else
        {
            dialogManager.OnNextStep();
        }
    }
    public void onWasteDestroyByPlayer(SystemId typeId)
    {
        if (typeId == SystemId.Cannette)
        {
            p1trigger = true;
        }

        if (typeId == SystemId.Glass)
        {
            p2trigger = true;
        }

        Debug.Log("waitingBothDestroy" + waitingBothDestroy);
        Debug.Log("p1hasInteract" + p1hasInteract);
        Debug.Log("p2hasInteract" + p2hasInteract);
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
                Debug.Log(" player has inte !!!!! p1 " + p1hasInteract + " p2 " + p2hasInteract);
                dialogManager.OnNextStep();
                waitingBothDestroy = false;
            }
        }
        else
        {
            Debug.Log(" no waitingBothDestroy !!!!!");
            dialogManager.OnNextStep();
            if (waitingP1Destroy) waitingP1Destroy = false;
            if (waitingP2Destroy) waitingP2Destroy = false;
        }
    }
    public void throwForP1()
    {
        waitingP1Destroy = true;
        throwWaste(cannette, spawn1.transform.position);
    }

    public void throwForP2()
    {
        waitingP2Destroy = true;
        throwWaste(glassBottle, spawn2.transform.position);
    }
    public void throwForBoth()
    {
        waitingBothDestroy = true;
        throwWaste(cannette, spawn1.transform.position);
        throwWaste(glassBottle, spawn2.transform.position);
    }

}
