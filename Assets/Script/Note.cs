using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    private int lineID = -1; // 라인 ID
    private Image noteImage;
    private bool isInitialized = false;
    private float fallTime = 0f;
    
    private Vector3 startPos;
    private Vector3 targetPos;

    private float startTime;
    
    void OnEnable()
    {
        if (noteImage == null)
            noteImage = GetComponent<Image>();

        noteImage.enabled = true;
    }
    
    public void Setup(NoteInfo info, float _fallTime, Vector3 _startPos, Vector3 _targetPos)
    {
        lineID = info.lane;
        startTime = info.startTime;
        
        fallTime = _fallTime;
        startPos = _startPos;
        targetPos = _targetPos;
        
        if (info.isLongNote)
        {
            SetLongNoteLayout(info);
        }
        
        isInitialized = true;
    }

    // 롱노트는 일반노트의 y값을 늘려준다.
    void SetLongNoteLayout(NoteInfo info)
    {
        float duration = (info.endTime - info.startTime); // 끝시간 - 시작시간
        float totalDistance = Vector2.Distance(startPos, targetPos); // 시작지점과 끝지점 계산
        float longNoteHeight = (duration / fallTime) * totalDistance; // 시간을 계산하여, 그만큼 Note Prefab의 y축을 늘려준다.
        
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, longNoteHeight);
        rect.pivot = new Vector2(0.5f, 0f); // 바닥에서 위로 올리는 것
        
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = new Vector2(rect.sizeDelta.x, longNoteHeight);
            collider.offset = new Vector2(0, longNoteHeight / 2); // 피봇이 바닥이므로 센터를 위로 올림
        }
    }

    public void HideNote()
    {
        noteImage.enabled = false;
        isInitialized = false;
        ObjectPool.instance.ReturnNote(this.gameObject, lineID);
    }

    void Update()
    {
        if (!isInitialized || !noteImage.enabled) return;

        // 현재 음악 시간을 SoundManager에서 가져옴 (ms 단위)
        float currentTime = SoundManager.Instance.GetMusicTime() * 1000f;

        float ratio = (startTime - currentTime) / fallTime;
        
        transform.localPosition = targetPos + (startPos - targetPos) * ratio;
        
        if (ratio < -0.2f) HideNote();
    }

    public bool GetNoteFlag()
    {
        return noteImage.enabled;
    }
    
    public void SetLineID(int id)
    {
        lineID = id;
    }

    public int GetLineID()
    {
        return lineID;
    }
}