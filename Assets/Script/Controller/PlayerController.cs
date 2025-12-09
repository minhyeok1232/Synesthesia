using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        GetKeyDown();
    }

    void GetKeyDown()
    {
        TimingManager timingManager = GameManager.Instance.GetTimingManager();
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            timingManager.CheckTiming(0);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            timingManager.CheckTiming(1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            timingManager.CheckTiming(2);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            timingManager.CheckTiming(3);
        }
        else
        {
            
        }
    }
}