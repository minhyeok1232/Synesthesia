using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : Singleton<SoundManager>
{
    public static bool isMusicEnd = false;
    public bool isPaused = false;
    
    // Sound
    [SerializeField] private Sound[] bgm = null;
    [SerializeField] private Sound[] sfx = null;
    
    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource sfxPlayer = null;
    
    
    
    void Start()
    {
        
    }
    void Update()
    {
        if (CenterFlame.musicStart && !bgmPlayer.isPlaying && isMusicEnd && !isPaused)
        {
            EndMusic();
        }
    }
    
    public void PlayBGM(string bgmName, bool playing = false)
    {
        if (!playing)
        {
            for (int i = 0; i < bgm.Length; i++)
            {
                if (bgm[i].name == bgmName)
                {
                    bgmPlayer.clip = bgm[i].clip;
                    bgmPlayer.Play();
                }
            }
        }
        else
            bgmPlayer.Play();
    }
    
    public void PlaySFX(string p_sfxName)
    {
        // for (int i = 0; i < sfx.Length; i++)
        // {
        //     if (p_sfxName == sfx[i].name)
        //     {
        //         for (int j = 0; j < sfxPlayer.Length; j++)
        //         {
        //             // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
        //             if (!sfxPlayer[j].isPlaying)
        //             {
        //                 sfxPlayer[j].clip = sfx[i].clip;
        //                 sfxPlayer[j].Play();
        //                 return;
        //             }
        //         }
        //         Debug.Log("모든 오디오 플레이어가 재생중입니다.");
        //         return;
        //     }
        // }
        // Debug.Log(p_sfxName + " 이름의 효과음이 없습니다.");
        // return;
    }
    
    // 환경 설정 -> 일시 정지
    public void PauseBGM()
    {
        isPaused = true; // "내가 의도적으로 멈춤" 표시
        bgmPlayer.Pause();
    }

    // 재개
    public void ResumeBGM()
    {
        isPaused = false; // "다시 재생" 표시
        bgmPlayer.UnPause();
    }

    // 완전 정지
    public void StopBGM()
    {
        CenterFlame.musicStart = false;

        bgmPlayer.Stop();
    }
    
    // 노래가 끝나면 결과창이 나올 수 있게,
    public void EndMusic()
    {
        StartCoroutine(CorEndMusic());
    }
        
    // 노래가 끝나면 결과창이 나올 수 있게,
    IEnumerator CorEndMusic()
    {
        yield return new WaitForSeconds(2.0f);
        
        // 몇초 기다리기. 이후는 SceneManager에서 처리
        SceneLoader sceneLoader = GameManager.Instance.GetSceneLoader();
        sceneLoader?.ChangeScene(2); // EndScene
    }
    
    public float GetMusicTime() // 현재 재생된 길이
    {
        if(bgmPlayer == null) return 0;
        return bgmPlayer.time;
    }

    public float GetMusicLength() // 전체 노래 길이
    {
        if(bgmPlayer == null || bgmPlayer.clip == null) return 0;
        return bgmPlayer.clip.length; 
    }
}
