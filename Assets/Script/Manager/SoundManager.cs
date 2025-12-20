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
        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfx[i].name == p_sfxName && sfx[i].clip != null)
            {
                // PlayOneShot을 쓰면 하나의 AudioSource에서 여러 소리가 겹쳐서 나옵니다. (리듬게임 필수)
                sfxPlayer.PlayOneShot(sfx[i].clip);
                return;
            }
        }
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
    
    #region [외부 음악 로드] - StreamingAssets 폴더에서 MP3, wav 파일을 비동기로 들고옴
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
    
    public IEnumerator LoadAllSFXFromFolder(string songName)
    {
        // 1. StreamingAssets 내 곡 전용 폴더 경로 설정
        string folderPath = Path.Combine(Application.streamingAssetsPath, songName);
    
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"[SFX 로드 실패] 폴더가 없습니다: {folderPath}");
            yield break;
        }

        // 2. 폴더 내의 모든 .wav 파일 목록 추출
        string[] filePaths = Directory.GetFiles(folderPath, "*.wav");

        // 3. 찾은 파일 개수만큼 Sound 배열 재설정 (이미지의 Sfx 배열 크기가 바뀝니다)
        sfx = new Sound[filePaths.Length];

        for (int i = 0; i < filePaths.Length; i++)
        {
            string fullPath = filePaths[i];
            string fileName = Path.GetFileNameWithoutExtension(fullPath); // 확장자 제외 이름
            string webPath = "file://" + fullPath;

            // 4. AudioType.WAV를 사용하여 로드
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(webPath, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    clip.name = fileName;

                    // 인스펙터 구조에 맞게 Sound 객체 생성
                    sfx[i] = new Sound {
                        name = fileName, // 나중에 PlaySFX("ride2") 처럼 찾을 이름
                        clip = clip
                    };
                    Debug.Log($"[SFX 로드 완료] {fileName}.wav");
                }
                else
                {
                    Debug.LogError($"[SFX 에러] {fileName} 로드 실패: {www.error}");
                }
            }
        }
    }
    #endregion
}
