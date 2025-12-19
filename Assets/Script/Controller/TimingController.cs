using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingController : MonoBehaviour
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
        if (!GameManager.Instance.MusicStart || notes == null) return;
        
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
            Note note = currentLineNotes[i].GetComponent<Note>();
            
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
    
    // 롱노트 - 키를 누르고 있는 동안
    public void CheckLongNoteHolding(int keyID)
    {
        if (boxNoteLists[keyID].Count > 0)
        {
            Note note = boxNoteLists[keyID][0].GetComponent<Note>();
        
            // 롱노트이고 현재 성공적으로 누르고 있는 상태라면
            if (note.info.isLongNote && note.isHolding)
            {
                // 1. 비주얼 업데이트 (노트가 판정선에서 타 들어가는 연출)
                note.UpdateLongNoteLife(); 
            
                // 2. 틱 콤보 (일정 시간마다 콤보 상승 로직은 여기서 실행)
                // GameManager.Instance.AddCombo(); (원하는 주기마다 실행되도록 처리)
            }
        }
    }

    // 롱노트 - 키를 떼었을 때 호출
    public void CheckLongNoteEnd(int lane)
    {
        if (boxNoteLists[lane].Count > 0)
        {
            Note note = boxNoteLists[lane][0].GetComponent<Note>();
        
            if (note.info.isLongNote && note.isHolding)
            {
                // 판정 시간(endTime) 이전에 너무 빨리 뗐는지 체크
                // 만약 너무 일찍 뗐다면 Miss 처리, 아니면 완료 처리
                note.isHolding = false;
            
                // 처리 완료 후 리스트에서 제거 및 파괴/반환
                DestroyNote(lane, boxNoteLists[lane][0]);
            }
        }
    }
}