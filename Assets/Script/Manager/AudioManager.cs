using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource bgmPlayer;
    public static bool isMusicEnd = false;
    
    void Start()
    {
        bgmPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (CenterFlame.musicStart && !bgmPlayer.isPlaying && !isMusicEnd)
        {
            StartCoroutine(EndMusic());
        }
    }

    public void PlayMusic()
    {
        bgmPlayer.Play();
    }

    // 환경 설정 -> 노래 일시정지
    public void StopMusic()
    {
        
    }
    
    // 노래가 끝나면 결과창이 나올 수 있게,
    IEnumerator EndMusic()
    {
        // TODO Scene이동
        isMusicEnd = true;

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
