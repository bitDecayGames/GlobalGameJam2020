using System;
using TMPro;
using UnityEngine;

public class PauseController : UnityEngine.MonoBehaviour
{
    public bool IsPaused;

    public TextMeshProUGUI TextMeshProUgui;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            IsPaused = !IsPaused;
        }
        
        if (IsPaused)
        {
            TextMeshProUgui.enabled = true;
        }
        else
        {
            TextMeshProUgui.enabled = false;
        }
    }
}