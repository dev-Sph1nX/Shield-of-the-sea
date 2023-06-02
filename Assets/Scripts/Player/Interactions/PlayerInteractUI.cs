using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInteractUI : MonoBehaviour
{

    [SerializeField] private GameObject player;
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
        GetComponent<Renderer>().enabled = true;
        gameObject.transform.position = player.transform.position + Vector3.up * 3;
    }

    private void Hide()
    {
        GetComponent<Renderer>().enabled = false;
    }

}