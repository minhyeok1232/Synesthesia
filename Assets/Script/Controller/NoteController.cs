using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteController : MonoBehaviour
{
    [Header("Note Prefabs")]
    [SerializeField] GameObject notePrefab = null; // 생성할 노트 프리팹
    
    [Header("Note Settings")]
    public Transform[] startPositions;   // 0, 1, 2, 3번 라인의 생성 위치

    public Transform[] endPositions;     // 0, 1, 2, 3번 라인 엔드 위치
    public float noteFallTime = 1000f;   // 노트가 내려오는 시간 (ms)

    void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetNoteController(this);
    }

    void Update()
    {
        
    }

    public void CreateNote(NoteInfo info)
    {
        // 1. 레인 위치 확인 (0~3번 레인 중 어디서 생성할지)
        if (info.lane < 0 || info.lane >= startPositions.Length) return;
        
        GameObject noteObj = ObjectPool.instance.GetNote(info.lane);
        
        noteObj.transform.localPosition = new Vector3(-10000, 0, 0);
        
        GameManager.Instance.GetTimingController()?.boxNoteLists[info.lane].Add(noteObj);
        if (noteObj == null) return;

        Image noteImage = noteObj.GetComponent<Image>();
        if (noteImage != null)
        {
            switch (info.lane)
            {
                case 0:
                    noteImage.color = new Color(244f / 255f, 67f / 255f, 67f / 255f, 0.95f);
                    break;
                case 1:
                    noteImage.color = new Color(33f / 255f, 33f / 255f, 248f / 255f, 0.95f);
                    break;
                case 2:
                    noteImage.color = new Color(1f, 148f / 255f, 50f / 255f, 0.95f);
                    break;
                case 3:
                    noteImage.color = new Color(70f / 255f, 243f / 255f, 38f / 255f, 0.95f);
                    break;
            }
        }
        
        Vector3 startPos = startPositions[info.lane].localPosition;
        Vector3 endPos = endPositions[info.lane].localPosition;
        
        Note note = noteObj.GetComponent<Note>(); 
        note?.Setup(info, noteFallTime, startPos, endPos);
        
        noteObj.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EffectController effectController = GameManager.Instance.GetEffectController();
        ComboController  comboController = GameManager.Instance.GetComboController();
        TimingController timingController = GameManager.Instance.GetTimingController();
        
        if (collision.CompareTag("Note"))
        {
            int lineID = collision.GetComponent<Note>().GetLineID();
            
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                timingController.MissRecord();
                effectController.JudgementEffect(4);
                comboController.ResetCombo();
            }
            
            if (lineID != -1 && lineID < timingController.boxNoteLists.Length)
            {
                timingController.boxNoteLists[lineID].Remove(collision.gameObject);
            }
            
            ObjectPool.instance.ReturnNote(collision.gameObject, lineID);
        }
    }
}