using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFlame : MonoBehaviour
{
    public static bool musicStart = false;

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager audioManager = GameManager.Instance.GetAudioManager();
        
        if (!musicStart)
        {
            if (collision.CompareTag("Note"))
            {
                musicStart = true;
                if (audioManager != null)
                    audioManager.PlayMusic();
                else
                    Debug.Log("AudioManager is Null !! ");
            }
        }
    }
}