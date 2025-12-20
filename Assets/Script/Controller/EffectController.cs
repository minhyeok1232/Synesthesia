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

    private void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetEffectController(this);
    }
    
    public void GetEffect(string p_name, int p_num)
    {
        judgementImage.sprite = judgementSprite[p_num];

        if (p_name == "LongNote")
        {
            judgementAnimator.Rebind();
        }
        judgementAnimator.SetTrigger(p_name);
    }

    public void NoteHitEffect(int _note)
    {
        noteHitAnimator.gameObject.transform.SetParent(noteHitTransform[_note]);
        noteHitAnimator.gameObject.transform.localPosition = Vector3.zero;
        noteHitAnimator.SetTrigger("Hit");
    }
}