using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{

    private void Start() 
    {
        UnitActionSystem.Instance.OnBusyChanged += OnBusyChanged;
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnBusyChanged(bool isBusy)
    {
        if(isBusy)
            Show();
        else
            Hide();
    }
}
