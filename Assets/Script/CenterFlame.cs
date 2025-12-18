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
}