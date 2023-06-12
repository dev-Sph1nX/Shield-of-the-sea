using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private Toggle m_DebugMode;
    private Toggle m_InverseX;
    private Toggle m_InverseZ;
    private Slider m_LevelGametime;
    private TMP_InputField m_Input;
    private bool debugMode;
    private bool inverseX;
    private bool inverseZ;
    private float levelGametime = 0f;
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject.GetComponent<Toggle>() && child.gameObject.name == "DebugMode")
            {
                m_DebugMode = child.gameObject.GetComponent<Toggle>();
            }
            if (child.gameObject.GetComponent<Slider>())
            {
                m_LevelGametime = child.gameObject.GetComponent<Slider>();
            }
            if (child.gameObject.GetComponent<TMP_InputField>())
            {
                m_Input = child.gameObject.GetComponent<TMP_InputField>();
            }
            if (child.gameObject.GetComponent<Toggle>() && child.gameObject.name == "InverseX")
            {
                m_InverseX = child.gameObject.GetComponent<Toggle>();
            }
            if (child.gameObject.GetComponent<Toggle>() && child.gameObject.name == "InverseZ")
            {
                m_InverseZ = child.gameObject.GetComponent<Toggle>();
            }
        }
        //Add listener for when the state of the Toggle changes, to take action
        m_DebugMode.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_DebugMode);
        });

        //Initialise the Text to say the first state of the Toggle
        debugMode = m_DebugMode.isOn;
        levelGametime = m_LevelGametime.value;
        m_Input.text = Mathf.Round(levelGametime * 100) + " (s)";

        m_LevelGametime.onValueChanged.AddListener(delegate
       {
           SliderValueChanged(m_LevelGametime);
       });
        inverseX = m_InverseX.isOn;
        m_InverseX.onValueChanged.AddListener(delegate
         {
             InversedXValueChanged(m_InverseX);
         });
        m_InverseZ.onValueChanged.AddListener(delegate
        {
            InversedZValueChanged(m_InverseZ);
        });
        inverseZ = m_InverseZ.isOn;

    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        debugMode = m_DebugMode.isOn;
    }

    void InversedXValueChanged(Toggle change)
    {
        inverseX = m_InverseX.isOn;
    }
    void InversedZValueChanged(Toggle change)
    {
        inverseZ = m_InverseZ.isOn;
    }

    void SliderValueChanged(Slider change)
    {
        levelGametime = m_LevelGametime.value;
        m_Input.text = Mathf.Round(levelGametime * 100) + " (s)";
    }


    public void OnStart()
    {
        StaticClass.CrossSceneInverseX = inverseX;
        StaticClass.CrossSceneInverseZ = inverseZ;
        StaticClass.CrossSceneGameTime = levelGametime * 100;
        StaticClass.CrossSceneDebugMode = debugMode;
        SceneManager.LoadScene("0-Lobby");
    }
}

public static class StaticClass
{
    public static bool CrossSceneInverseX { get; set; }
    public static bool CrossSceneInverseZ { get; set; }
    public static bool CrossSceneDebugMode { get; set; }
    public static float CrossSceneGameTime { get; set; }
}