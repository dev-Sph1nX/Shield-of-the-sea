using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInteractUI : MonoBehaviour
{

    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteraction playerInteract;
    // [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    private void Update()
    {
        if (playerInteract.GetInteractableObject() != null)
        {
            Show(playerInteract.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }

    private void Show(IInteractable interactable)
    {
        containerGameObject.SetActive(true);
        // interactTextMeshProUGUI.text = interactable.GetInteractIcon();
    }

    private void Hide()
    {
        containerGameObject.SetActive(false);
    }

}