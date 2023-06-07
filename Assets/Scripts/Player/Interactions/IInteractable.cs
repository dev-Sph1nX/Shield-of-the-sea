using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    void Interact(SystemId id); // Transform interactorTransform
    IconType GetInteractIcon();
    Transform GetTransform();
    string GetId();
    bool isInteractable();

}

public enum IconType
{
    Take,
    Release
}