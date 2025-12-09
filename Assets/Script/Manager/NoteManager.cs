using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [SerializeField] GameObject goNote = null; // 생성할 노트 프리팹

    void Start()
    {

    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        EffectManager effectManager = GameManager.Instance.GetEffectManager();
        ComboManager  comboManager  = GameManager.Instance.GetComboManager();
        TimingManager timingManager = GameManager.Instance.GetTimingManager();
        
        if (collision.CompareTag("Note"))
        {
            int lineID = collision.GetComponent<Note>().GetLineID();
            
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                effectManager.JudgementEffect(4);
                comboManager.ResetCombo();
            }
            
            if (lineID != -1 && lineID < timingManager.boxNoteLists.Length)
            {
                timingManager.boxNoteLists[lineID].Remove(collision.gameObject);
            }
            
            ObjectPool.instance.ReturnNote(collision.gameObject, lineID);
        }
    }
}