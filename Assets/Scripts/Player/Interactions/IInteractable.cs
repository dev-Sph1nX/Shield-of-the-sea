using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    void Interact(SystemId id); // Transform interactorTransform
    Transform GetTransform();
    bool isInteractable();
    string getId();
    int getRange();

}

public enum IconType
{
    Take,
    Release
}