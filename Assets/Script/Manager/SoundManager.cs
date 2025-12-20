using System;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : Singleton<SoundManager>
{
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
        if (GameManager.Instance.CurrentState == MusicState.Playing)
        {
            if (bgmPlayer.clip != null && bgmPlayer.time >= bgmPlayer.clip.length - 0.1f)
            {
                GameManager.Instance.CurrentState = MusicState.Finished;
            }
        }
        
        if (GameManager.Instance.CurrentState == MusicState.Finished)
        {
            if (bgmPlayer.isPlaying) 
            {
                bgmPlayer.Stop(); 
                EndMusic();
            }
        }
    }
    
    public void PlayBGM(string bgmName, int startTimeMs = 0, bool playing = false)
    {
        if (!playing)
        {
            for (int i = 0; i < bgm.Length; i++)
            {
                if (bgm[i].name == bgmName)
                {
                    bgmPlayer.clip = bgm[i].clip;
                    bgmPlayer.time = startTimeMs / 1000f; 
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
        GameManager.Instance.CurrentState = MusicState.Paused;
        bgmPlayer.Pause();
    }

    // 재개
    public void ResumeBGM()
    {
        GameManager.Instance.CurrentState = MusicState.Playing;
        bgmPlayer.UnPause();
    }

    // 완전 정지
    public void StopBGM()
    {
        bgmPlayer.clip = null;
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
    
    #region [외부 음악 로드] - StreamingAssets 폴더에서 MP3 파일을 비동기로 들고옴
    public IEnumerator LoadBGMFromStreamingAssets(string songName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (bgm[i].name == songName && bgm[i].clip != null)
            {
                yield break;
            }
        }

        // 각 노래의 제목에 맞는 폴더명 설정 (경로 : Asset/StreamingAssets/노래제목/) 
        string path = "file://" + Path.Combine(Application.streamingAssetsPath, songName, "audio.mp3");

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
        {
            Debug.Log($"[로드 시작] 시도 중인 경로: {path}");

            yield return www.SendWebRequest();
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[로드 실패] 에러 내용: {www.error}");
                Debug.LogError($"[참고] 응답 코드: {www.responseCode}");
            }
            else
            {
                Debug.Log("[로드 성공] 파일을 정상적으로 가져왔습니다.");

                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
        
                if (clip == null)
                {
                    Debug.LogError("[데이터 오류] AudioClip을 생성하지 못했습니다. 파일이 깨졌거나 형식이 다를 수 있습니다.");
                }
                else
                {
                    clip.name = songName;
                    bool isAssigned = false;
                    
                    for (int i = 0; i < bgm.Length; i++)
                    {
                        if (bgm[i].name == songName)
                        {
                            bgm[i].clip = clip;
                            Debug.Log($"[매칭 성공] SoundManager의 bgm[{i}] 슬롯에 {songName} 할당 완료!");
                            isAssigned = true;
                            break;
                        }
                    }

                    if (!isAssigned)
                    {
                        Debug.LogWarning($"[매칭 실패] SoundManager bgm 배열에서 '{songName}'이라는 이름을 찾을 수 없습니다. 인스펙터를 확인하세요.");
                    }
                }
            }
        }
    }
    #endregion
}
