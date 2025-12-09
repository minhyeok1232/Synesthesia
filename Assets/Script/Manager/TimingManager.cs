using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : Singleton<TimingManager>
{
    // Time
    public int bpm = 0;      // 리듬게임 비트 단위. 1분당 몇 비트인지.
    double currentTime = 0d; // 리듬 게임은 오차 적은게 중요해서 float보단 double

    // GameObject
    public List<GameObject>[] boxNoteLists = null; 

    // Transform
    [SerializeField] Transform[] appear = null; // 노트 생성 위치 오브젝트
    [SerializeField] Transform[] center = null; // 판정 범위의 중심
    [SerializeField] RectTransform[] rect = null; // 다양한 판정 범위
    [SerializeField] private Transform[] notes = null; // 노트를 관리하는 게임오브젝트
    
    Vector2[] timingBoxs = null; // 판정 범위 최소값 x, 최대값 y
    
    void Start()
    {
        timingBoxs = new Vector2[rect.Length];
        
        // 라인 별 노트 리스트 초기화
        boxNoteLists = new List<GameObject>[appear.Length];
        for (int i = 0; i < appear.Length; i++)
        {
            boxNoteLists[i] = new List<GameObject>();

            for (int j = 0; j < rect.Length; j++)
            {
                timingBoxs[i].Set(center[i].localPosition.x - rect[j].rect.width / 2,
                    center[i].localPosition.x + rect[j].rect.width / 2);
            }
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        
        if (currentTime >= 60d / bpm)
        {
            // 랜덤 패턴 
            int randomKeyID = Random.Range(0, appear.Length);
        
            // ObjectPool에서 해당 라인의 큐를 사용하여 노트를 가져옵니다.
            GameObject note = ObjectPool.instance.GetNote(randomKeyID);
            
            note.GetComponent<Note>().SetLineID(randomKeyID); // 몇번 째 키 ID 저장
            
            note.transform.position = appear[randomKeyID].position;
            note.SetActive(true);
            
            boxNoteLists[randomKeyID].Add(note);
            
            currentTime -= 60d / bpm;  // currentTime = 0 으로 리셋해주면 안된다. 
        }    
    }
    public void CheckTiming(int keyID)
    {
        EffectManager effectManager = GameManager.Instance.GetEffectManager();
        ComboManager  comboManager  = GameManager.Instance.GetComboManager();
        ScoreManager  scoreManager  = GameManager.Instance.GetScoreManager();
        
        List<GameObject> currentLineNotes = boxNoteLists[keyID];
        
        for(int i = 0; i < currentLineNotes.Count; i++)
        {
            float t_notePosX = currentLineNotes[i].transform.localPosition.x;
    
            // 판정 순서 : Perfect -> Cool -> Good -> Bad
            for (int j = 0; j < timingBoxs.Length; j++)
            {
                Debug.Log(timingBoxs[j].x);
                Debug.Log(timingBoxs[j].y);

                
                if (timingBoxs[j].x <= t_notePosX && t_notePosX <= timingBoxs[j].y)
                {
                    // 노트 제거
                    currentLineNotes[i].GetComponent<Note>().HideNote();
                    currentLineNotes.RemoveAt(i);
                    
                    // 이펙트 연출
                    if (j < timingBoxs.Length - 1)
                    {
                        effectManager.NoteHitEffect();
                        scoreManager.AnimPlayScore();
                    }
    
                    effectManager.JudgementEffect(j);
                    scoreManager.IncreaseScore(j);
                    return;
                }
            }
        }
        
        comboManager.ResetCombo();
        effectManager.JudgementEffect(timingBoxs.Length);
    }
}