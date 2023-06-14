using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenningLevelModal : MonoBehaviour, InnerModalScript
{
    [SerializeField] TextMeshProUGUI p1text;
    [SerializeField] TextMeshProUGUI p2text;

    private bool _isDone = false, _isShow = true, p1interact = false, p2interact = false;

    void Start()
    {

    }

    void Update()
    {
        if (_isShow)
        {
            if (p1interact)
            {
                p1text.enabled = true;
            }
            if (p2interact)
            {
                p2text.enabled = true;
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
