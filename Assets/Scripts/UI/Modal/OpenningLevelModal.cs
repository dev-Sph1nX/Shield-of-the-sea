using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenningLevelModal : MonoBehaviour, InnerModalScript
{
    [SerializeField] Image p1;
    [SerializeField] Image p2;

    private bool _isDone = false, _isShow = true, p1interact = false, p2interact = false;

    void Update()
    {
        if (_isShow)
        {
            if (p1interact)
            {
                p1.enabled = true;
            }
            if (p2interact)
            {
                p2.enabled = true;
            }

            if (p1interact && p2interact)
            {
                _isDone = true;
            }
        }
    }

    public bool isDone()
    {
        return _isDone;
    }
    public void isShow()
    {
        _isShow = true;
    }


    public void OnPlayer1Interact()
    {
        p1interact = true;
    }
    public void OnPlayer2Interact()
    {
        p2interact = true;
    }
}
