using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField] GameObject goNote = null; // 생성할 노트 프리팹

    void Start()
    {
        GameManager.Instance.SetNoteController(this);
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