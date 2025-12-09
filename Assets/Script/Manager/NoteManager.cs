using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{

    
    [SerializeField] GameObject goNote = null; // 생성할 노트 프리팹

    TimingManager theTimingManager;
    EffectManager theEffectManager;
    private ComboManager theComboManager;

    void Start()
    {
        theEffectManager = FindObjectOfType<EffectManager>();
        theTimingManager = GetComponent<TimingManager>();
        theComboManager = FindObjectOfType<ComboManager>();
    }
}