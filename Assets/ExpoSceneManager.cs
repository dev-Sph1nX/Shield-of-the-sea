using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpoSceneManager : MonoBehaviour
{
    [Header("Scene Transition")]
    [SerializeField] private Animator sceneAnimator = null;

    private Image mask = null;
    // Start is called before the first frame update
    void Start()
    {
        sceneAnimator.gameObject.SetActive(true);
        sceneAnimator.SetTrigger("Enter");
        
        foreach (Transform child in sceneAnimator.gameObject.transform)
        {
            if (child.gameObject.tag == "TransitionImage")
                mask = child.gameObject.GetComponent<Image>();
        }
        
        Invoke("HideMask", 1f);
    }
    
    private void HideMask()
    {
        mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
