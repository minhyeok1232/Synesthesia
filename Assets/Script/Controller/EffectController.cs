using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator = null;
    [SerializeField] Transform[] noteHitTransform;

    [SerializeField] Animator judgementAnimator = null;
    [SerializeField] Image judgementImage = null;
    [SerializeField] Sprite[] judgementSprite = null;

    string hit = "Hit";

    private void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetEffectController(this);
    }

    public void JudgementEffect(int p_num)
    {
        judgementImage.sprite = judgementSprite[p_num];
        judgementAnimator.SetTrigger(hit);
    }

    public void NoteHitEffect(int _note)
    {
        noteHitAnimator.gameObject.transform.SetParent(noteHitTransform[_note]);
        noteHitAnimator.gameObject.transform.localPosition = Vector3.zero;
        noteHitAnimator.SetTrigger(hit);
    }
}