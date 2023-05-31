using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private Animator sceneAnimator = null;
    [SerializeField] private Image mask = null;


    // Start is called before the first frame update
    void Start()
    {
        sceneAnimator.SetTrigger("Enter");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onEnterAnimationFinish()
    {
        Debug.Log("bcjklilo");
        mask.color = new Color(1.0f, 1.0f, 1.0f, 0);
    }
}
