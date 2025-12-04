using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0; // 리듬게임 비트 단위. 1분당 몇 비트인지.
    double currentTime = 0d; // 리듬 게임은 오차 적은게 중요해서 float보단 double

    [SerializeField] Transform tfNoteAppear = null; // 노트 생성 위치 오브젝트
    [SerializeField] GameObject goNote = null; // 생성할 노트 프리팹

    TimingManager theTimingManager;
    EffectManager theEffectManager;

    void Start()
    {
        theTimingManager = GetComponent<TimingManager>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60d / bpm)
        {
            GameObject note = ObjectPool.instance.noteQueue.Dequeue();
            note.transform.position = tfNoteAppear.position;
            note.SetActive(true);
            
            theTimingManager.boxNoteList.Add(note);
            
            currentTime -= 60d / bpm;  // currentTime = 0 으로 리셋해주면 안된다. 
        }            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            if (collision.GetComponent<Note>().GetNoteFlag())
                theEffectManager.JudgementEffect(4);
            
            theTimingManager.boxNoteList.Remove(collision.gameObject);
            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }
}