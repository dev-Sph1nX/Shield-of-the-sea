using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    void Interact(PlayerId playerId); // Transform interactorTransform
    IconType GetInteractIcon();
    Transform GetTransform();
    string GetId();

}

public enum IconType
{
    Take,
    Release
}