using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LaneHighlighter[] laneEffects; // 인스펙터에서 4개 할당
    
    void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetPlayerController(this);
    }

    void Update()
    {
        GetKeyDown();
    }

    void GetKeyDown()
    {
        TimingController timingController = GameManager.Instance.GetTimingController();
        
        KeyCode[] keys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

        for (int i = 0; i < keys.Length; i++)
        {
            // 1. 키를 처음 눌렀을 때 (단타 판정 및 롱노트 시작)
            if (Input.GetKeyDown(keys[i]))
            {
                timingController.CheckTiming(i);
                SoundManager.Instance.PlaySFX("hitsound");

                laneEffects[i].OnPress();
            }
        
            // 2. 키를 떼었을 때 (롱노트 끝 판정)
            if (Input.GetKeyUp(keys[i]))
            {
                timingController.CheckLongNoteEnd(i);
                
                laneEffects[i].OnRelease();
            }

            // 3. 키를 꾹 누르고 있는 동안 (콤보 상승 및 비주얼 처리)
            if (Input.GetKey(keys[i]))
            {
                timingController.CheckLongNoteHolding(i);
            }
        }
    }
}