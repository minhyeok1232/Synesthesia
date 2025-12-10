using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.SetPlayerController(this);
    }

    void Update()
    {
        GetKeyDown();
    }

    void GetKeyDown()
    {
        TimingController timingController = GameManager.Instance.GetTimingController();

        Debug.Log(timingController);
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            timingController.CheckTiming(0);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            timingController.CheckTiming(1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            timingController.CheckTiming(2);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            timingController.CheckTiming(3);
        }
        else
        {
            
        }
    }
}