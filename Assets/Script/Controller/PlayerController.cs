using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    TimingManager theTimingManager;

    void Start()
    {
        theTimingManager = FindObjectOfType<TimingManager>();
    }

    void Update()
    {
        GetKeyDown();
    }

    void GetKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            theTimingManager.CheckTiming(0);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            theTimingManager.CheckTiming(1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            theTimingManager.CheckTiming(2);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            theTimingManager.CheckTiming(3);
        }
        else
        {
            
        }
    }
}