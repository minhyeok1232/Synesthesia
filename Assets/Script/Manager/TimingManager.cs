using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public int bpm = 0;      // 리듬게임 비트 단위. 1분당 몇 비트인지.
    double currentTime = 0d; // 리듬 게임은 오차 적은게 중요해서 float보단 double

    public List<GameObject>[] boxNoteLists = null; 

    [SerializeField] Transform[] tfNoteAppear = null; // 노트 생성 위치 오브젝트
    [SerializeField] Transform center = null; // 판정 범위의 중심
    [SerializeField] RectTransform[] timingRect = null; // 다양한 판정 범위
    
    Vector2[] timingBoxs = null; // 판정 범위 최소값 x, 최대값 y

    private EffectManager theEffect;
    private ScoreManager theScore;
    private ComboManager theCombo;

    
    void Start()
    {
        theEffect = FindObjectOfType<EffectManager>();
        theScore = FindObjectOfType<ScoreManager>();
        theCombo = FindObjectOfType<ComboManager>();
        
        timingBoxs = new Vector2[timingRect.Length];

        for (int i = 0; i < timingRect.Length; i++)
        {
            timingBoxs[i].Set(center.localPosition.x - timingRect[i].rect.width / 2,
                center.localPosition.x + timingRect[i].rect.width / 2);
        }
        
        // 라인 별 노트 리스트 초기화
        boxNoteLists = new List<GameObject>[tfNoteAppear.Length];
        for (int i = 0; i < tfNoteAppear.Length; i++)
        {
            boxNoteLists[i] = new List<GameObject>();
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        
        if (currentTime >= 60d / bpm)
        {
            // 랜덤 패턴 
            int randomKeyID = Random.Range(0, tfNoteAppear.Length);
        
            // ObjectPool에서 해당 라인의 큐를 사용하여 노트를 가져옵니다.
            GameObject note = ObjectPool.instance.GetNote(randomKeyID);
            
            Debug.Log("note : " + note);
            
            note.GetComponent<Note>().SetLineID(randomKeyID); // 몇번 째 키 ID 저장
            
            note.transform.position = tfNoteAppear[randomKeyID].position;
            note.SetActive(true);
            
            boxNoteLists[randomKeyID].Add(note);
            
            currentTime -= 60d / bpm;  // currentTime = 0 으로 리셋해주면 안된다. 
        }    
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            int lineID = collision.GetComponent<Note>().GetLineID();
            
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                theEffect.JudgementEffect(4);
                theCombo.ResetCombo();
            }
            
            if (lineID != -1 && lineID < boxNoteLists.Length)
            {
                boxNoteLists[lineID].Remove(collision.gameObject);
            }
            
            ObjectPool.instance.ReturnNote(collision.gameObject, lineID);
        }
    }
    
    public void CheckTiming(int keyID)
    {
        List<GameObject> currentLineNotes = boxNoteLists[keyID];
        
        for(int i = 0; i < currentLineNotes.Count; i++)
        {
            float t_notePosX = currentLineNotes[i].transform.localPosition.x;
    
            // 판정 순서 : Perfect -> Cool -> Good -> Bad
            for (int j = 0; j < timingBoxs.Length; j++)
            {
                if (timingBoxs[j].x <= t_notePosX && t_notePosX <= timingBoxs[j].y)
                {
                    // 노트 제거
                    currentLineNotes[i].GetComponent<Note>().HideNote();
                    currentLineNotes.RemoveAt(i);
                    
                    // 이펙트 연출
                    if (j < timingBoxs.Length - 1)
                    {
                        theEffect.NoteHitEffect();
                        theScore.animPlayScore();
                    }
    
                    theEffect.JudgementEffect(j);
                    theScore.IncreaseScore(j);
                    return;
                }
            }
        }
        
        theCombo.ResetCombo();
        theEffect.JudgementEffect(timingBoxs.Length);
    }
}