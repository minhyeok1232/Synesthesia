using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingController : MonoBehaviour
{
    // Time
    public int bpm = 0;      // 리듬게임 비트 단위. 1분당 몇 비트인지.
    double currentTime = 0d; // 리듬 게임은 오차 적은게 중요해서 float보단 double
    
    // Long Note Tick 
    private float comboTickTimer = 0f;
    private const float comboTickInterval = 0.1f;
    
    // GameObject
    public List<GameObject>[] boxNoteLists = null; 

    // Transform
    [SerializeField] Transform[] appear = null; // 노트 생성 위치 오브젝트
    [SerializeField] Transform[] center = null; // 판정 범위의 중심
    [SerializeField] RectTransform[] rect = null; // 다양한 판정 범위
    
    int[] judgementRecord = new int[5];
    
    Vector2[] timingBoxs = null; // 판정 범위 최소값 x, 최대값 y
    
    private Song song;
    
    private List<NoteInfo> notes; // 전체 노트
    private int noteIndex = 0;    // 노트 인덱스
    
    void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetTimingController(this);

        if (GameManager.Instance != null)
            song = GameManager.Instance.GetCurrentSong();
    }

    void OnEnable()
    {
        if (song != null)
        {
            SoundManager.Instance.PlayBGM(song.name);
        }
    }
    
    void Start()
    {
        timingBoxs = new Vector2[rect.Length]; // timingBoxs[4]
        
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
        if (GameManager.Instance.CurrentState != MusicState.Playing || notes == null) return;
        
        // ms 단위 -> 음악 시간 가져오기
        float currentTime = SoundManager.Instance.GetMusicTime() * 1000f;
        
        while (noteIndex < notes.Count)
        {
            // (현재 시간 + 미리 보여줄 시간)이 노트의 시작 시간보다 크면 소환!
            if (currentTime + 1000 >= notes[noteIndex].startTime)
            {
                SpawnNote(notes[noteIndex]);
                noteIndex++; // 다음 노트로 번호표 넘김
            }
            else
                break;
        }
        
        for (int i = 0; i < boxNoteLists.Length; i++)
        {
            if (boxNoteLists[i].Count > 0)
            {
                GameObject targetNoteObj = boxNoteLists[i][0];
                Note note = targetNoteObj.GetComponent<Note>();
                float t_notePosY = targetNoteObj.transform.localPosition.y;

                // Bad 하단 영역
                float missLine = timingBoxs[3].x;

                // 1. 영역을 벗어나는 즉시 Miss 판정
                if (!note.isHolding && t_notePosY < missLine)
                {
                    MissRecord();
                    GameManager.Instance.GetComboController().ResetCombo();
                    GameManager.Instance.GetEffectController().GetEffect("Hit", 4); // Miss 인덱스
                
                    boxNoteLists[i].RemoveAt(0);

                    // 일반 노트는 여기서 바로 제거 가능
                    if (!note.info.isLongNote)
                    {
                        note.HideNote();
                    }
                }
            }
        }
    }
    
    public void Initialized()
    {
        notes = GameManager.Instance.GetNoteList();
        noteIndex = 0;
        
        for (int i = 0; i < rect.Length; i++)
            judgementRecord[i] = 0;
    }
    
    // 각 노트의 정의는 NoteController 에 위임한다.
    void SpawnNote(NoteInfo info)
    {
        GameManager.Instance.GetNoteController().CreateNote(info);
    }
    
    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()
    {
        judgementRecord[4]++;  // Miss의 판정 기록
    }
    
    public void CheckTiming(int keyID)
    {
        List<GameObject> currentLineNotes = boxNoteLists[keyID];
        
        for(int i = 0; i < currentLineNotes.Count; i++)
        {
            float t_notePosY = currentLineNotes[i].transform.localPosition.y;
            Note note = currentLineNotes[i].GetComponent<Note>();
            
            // 판정 순서 : Perfect -> Cool -> Good -> Bad
            for (int j = 0; j < timingBoxs.Length; j++)
            {
                if (timingBoxs[j].x <= t_notePosY && t_notePosY <= timingBoxs[j].y)
                {
                    if (note.info.isLongNote)
                    {
                        note.isHolding = true; 
                        HandleJudgement(j, keyID);
                    }
                    else
                    {
                        // 노트 제거
                        note.HideNote();
                        currentLineNotes.RemoveAt(i);
                        HandleJudgement(j, keyID);
                    }

                    return;
                }
            }
        }
    }
    
    // 롱노트 - 키를 누르고 있는 동안
    public void CheckLongNoteHolding(int keyID)
    {
        if (boxNoteLists[keyID].Count > 0)
        {
            GameObject noteObj = boxNoteLists[keyID][0];
            Note note = boxNoteLists[keyID][0].GetComponent<Note>();

            if (note.info.isLongNote && note.isHolding)
            {
                float noteTopY = noteObj.transform.localPosition.y + (note.GetComponent<RectTransform>().rect.height);
                float missLine = timingBoxs[3].x;
                
                if (noteTopY < missLine)
                {
                    note.isHolding = false;
                    boxNoteLists[keyID].RemoveAt(0);
                    // 롱노트를 정상적으로 다 처리했으므로 여기서 필요한 종료 연출 호출
                    return;
                }
                
                comboTickTimer += Time.deltaTime;
                
                if (comboTickTimer >= comboTickInterval)
                {
                    // 콤보 및 점수 추가
                    GameManager.Instance.GetComboController().IncreaseCombo();
                    GameManager.Instance.GetScoreController().IncreaseScore(0);
                    GameManager.Instance.GetEffectController()?.GetEffect("LongNoteHit", 0);
                    
                    comboTickTimer -= comboTickInterval; 
                }
            }
        }
    }

    // 롱노트 - 키를 떼었을 때 호출
    public void CheckLongNoteEnd(int keyID)
    {
        if (boxNoteLists[keyID].Count > 0)
        {
            Note note = boxNoteLists[keyID][0].GetComponent<Note>();
    
            if (note.info.isLongNote && note.isHolding)
            {
                note.isHolding = false;
                GameManager.Instance.GetEffectController()?.GetEffect("Up", 0);
                
                float currentTime = SoundManager.Instance.GetMusicTime() * 1000f;
            
                // 너무 일찍 뗐을 때 (예: 종료 100ms 전보다 더 빨리 뗌)
                if (currentTime < note.info.endTime - 100f) 
                {
                    // Miss 처리
                    GameManager.Instance.GetComboController().ResetCombo();
                    GameManager.Instance.GetEffectController().GetEffect("Hit", 4); // Miss
                    MissRecord();
                }
                
                boxNoteLists[keyID].RemoveAt(0);
            }
        }
    }
    
    // 판정 연출 분리
    private void HandleJudgement(int judgeIndex, int keyID)
    {
        EffectController effectController = GameManager.Instance.GetEffectController();
        ComboController comboController = GameManager.Instance.GetComboController();
        ScoreController scoreController = GameManager.Instance.GetScoreController();

        if (judgeIndex < timingBoxs.Length - 1)
        {
            effectController.NoteHitEffect(keyID);
            scoreController.AnimPlayScore();
        }
        effectController.GetEffect("Hit", judgeIndex);
        scoreController.IncreaseScore(judgeIndex);
        judgementRecord[judgeIndex]++;

        if (judgeIndex == 3) comboController.ResetCombo();
    }
}