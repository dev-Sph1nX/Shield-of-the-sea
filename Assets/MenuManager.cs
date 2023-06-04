using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnStart()
    {
        SceneManager.LoadScene("0-Lobby");
    }
}
