using System;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class RingController : MonoBehaviour
{
    public JobManager MyJobManager;
    public Image Left;
    public Image Right;
    public float Timer;
    public bool ringLeft;
    
    private void Start()
    {
        MyJobManager = FindObjectOfType<JobManager>();
    }

    private void Update()
    {
        Timer += Time.deltaTime;

        if (Timer > 1)
        {
            Timer = 0;
            ringLeft = !ringLeft;
        }
        
        if (MyJobManager.CallIsWaiting)
        {
            if (ringLeft)
            {
                Left.enabled = true;
                Right.enabled = false;
            }
            else
            {
                Left.enabled = false;
                Right.enabled = true;
            }
        }
        else
        {
            Left.enabled = false;
            Right.enabled = false;
        }
    }
}