using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingController : MonoBehaviour
{
    // Time
    public int bpm = 0;      // 리듬게임 비트 단위. 1분당 몇 비트인지.
    double currentTime = 0d; // 리듬 게임은 오차 적은게 중요해서 float보단 double

    private float noteFallTime = 5.0f; // 노트가 떨어지는 시간
    
    // GameObject
    public List<GameObject>[] boxNoteLists = null; 

    // Transform
    [SerializeField] Transform[] appear = null; // 노트 생성 위치 오브젝트
    [SerializeField] Transform[] center = null; // 판정 범위의 중심
    [SerializeField] RectTransform[] rect = null; // 다양한 판정 범위
    [SerializeField] private Transform[] notes = null; // 노트를 관리하는 게임오브젝트
    
    int[] judgementRecord = new int[5];
    
    Vector2[] timingBoxs = null; // 판정 범위 최소값 x, 최대값 y
    
    void Start()
    {
        GameManager.Instance.SetTimingController(this);
        
        timingBoxs = new Vector2[rect.Length];
        
        // 라인 별 노트 리스트 초기화
        boxNoteLists = new List<GameObject>[appear.Length];
        for (int i = 0; i < appear.Length; i++)
        {
            boxNoteLists[i] = new List<GameObject>();

            for (int j = 0; j < rect.Length; j++)
            {
                timingBoxs[j].Set(center[i].localPosition.x - rect[j].rect.width / 2,
                    center[i].localPosition.x + rect[j].rect.width / 2);
            }
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        
        float currentMusicTime = AudioManager.Instance.GetMusicTime();
        float totalMusicLength = AudioManager.Instance.GetMusicLength();
        
        if (currentMusicTime < totalMusicLength - noteFallTime)
        {
            if (currentTime >= 60d / bpm && !AudioManager.isMusicEnd)
            {
                // 랜덤 패턴 
                int randomKeyID = Random.Range(0, appear.Length);

                // ObjectPool에서 해당 라인의 큐를 사용하여 노트를 가져옵니다.
                GameObject note = ObjectPool.instance.GetNote(randomKeyID);

                note.GetComponent<Note>().SetLineID(randomKeyID); // 몇번 째 키 ID 저장

                note.transform.position = appear[randomKeyID].position;
                note.SetActive(true);

                boxNoteLists[randomKeyID].Add(note);

                currentTime -= 60d / bpm; // currentTime = 0 으로 리셋해주면 안된다. 
            }
        }
    }
    
    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()
    {
        Debug.Log("Miss");
        judgementRecord[4]++;  // Miss의 판정 기록
    }
    
    public void CheckTiming(int keyID)
    {
        EffectController effectController = GameManager.Instance.GetEffectController();
        ComboController  comboController  = GameManager.Instance.GetComboController();
        ScoreController  scoreController  = GameManager.Instance.GetScoreController();
        
        List<GameObject> currentLineNotes = boxNoteLists[keyID];
        
        for(int i = 0; i < currentLineNotes.Count; i++)
        {
            float t_notePosY = currentLineNotes[i].transform.localPosition.y;
            
            // 판정 순서 : Perfect -> Cool -> Good -> Bad
            for (int j = 0; j < timingBoxs.Length; j++)
            {
                if (timingBoxs[j].x <= t_notePosY && t_notePosY <= timingBoxs[j].y)
                {
                    // 노트 제거
                    currentLineNotes[i].GetComponent<Note>().HideNote();
                    currentLineNotes.RemoveAt(i);
                    
                    // 이펙트 연출
                    if (j < timingBoxs.Length - 1)
                    {
                        effectController.NoteHitEffect(keyID);
                        scoreController.AnimPlayScore();
                    }
    
                    effectController.JudgementEffect(j);
                    scoreController.IncreaseScore(j);
                    judgementRecord[j]++;  // 판정 기록

                    if (j == 3)
                    {
                        comboController.ResetCombo();
                    }
                    return;
                }
            }
        }
        
        MissRecord();
        comboController.ResetCombo();
        effectController.JudgementEffect(timingBoxs.Length);
    }
}