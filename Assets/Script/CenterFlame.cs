using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFlame : MonoBehaviour
{
    public static bool musicStart = false;

    public string bgmName = "";
    
    void Start()
    {
        bgmName = GameManager.Instance.GetCurrentSong().name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!musicStart)
        {
            if (collision.CompareTag("Note"))
            {
                musicStart = true;
                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlayBGM(bgmName, true);
                else
                    Debug.Log("AudioManager is Null !! ");
            }
        }
    }
}