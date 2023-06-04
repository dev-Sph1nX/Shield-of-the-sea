using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinInteractable : MonoBehaviour, IInteractable
{
    [Header("Id")]
    [SerializeField] SystemId wasteAcceptedType;

    public string GetId()
    {
        return wasteAcceptedType.ToString();
    }

    public IconType GetInteractIcon()
    {
        throw new System.NotImplementedException();
    }

    public Transform GetTransform()
    {
        throw new System.NotImplementedException();
    }

    public void Interact(SystemId id)
    {

    }

}
