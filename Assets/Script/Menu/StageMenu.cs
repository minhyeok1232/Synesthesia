using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Song
{
    public string name;
    public string composer;
    public int bpm;
    public Sprite sprite;
    public int previewTime; // 하이라이트 시간
}

public class StageMenu : MonoBehaviour
{
    // Song Class
    [Header("Song Class")]
    [SerializeField] private Song[] songList = null;
    [SerializeField] private Text txtSongName = null;
    [SerializeField] private Text txtComposer = null;
    [SerializeField] private Image imageDisk = null;
    
    [SerializeField] private GameObject TitleMenuUI = null;
    
    [Header("4 Buttons")] 
    [SerializeField] private Button btn_Back;
    [SerializeField] private Button btn_Play;
    [SerializeField] private Button btn_Next;
    [SerializeField] private Button btn_Prior;
    
    private int currentSong = 0;
    
    // StageMenu 열리면, 다시 첫번째 노래 재생목록으로 초기화
    private void OnEnable()
    {
        currentSong = 0; 
        SettingSong(); 
    }
    
    private void Start()
    {
        if (btn_Back != null)
            btn_Back.onClick.AddListener(() => BtnBack());
        
        if (btn_Play != null)
            btn_Play.onClick.AddListener(() => BtnPlay());
        
        if (btn_Next != null)
            btn_Next.onClick.AddListener(() => NextSong());
        
        if (btn_Prior != null)
            btn_Prior.onClick.AddListener(() => PriorSong());


        SettingSong();
    }
    
    public void NextSong()
    {
        if (++currentSong > songList.Length - 1)
            currentSong = 0;
        SettingSong();
    }

    public void PriorSong()
    {
        if (--currentSong < 0)
            currentSong = songList.Length - 1;
        SettingSong();
    }

    private void SettingSong() // 이전버튼 & 다음버튼을 누르면 노래의 정보를 들고온다. (위의 Song 구조체에서 받아옴)
    {
        SoundManager.Instance.StopBGM();
        
        txtSongName.text = songList[currentSong].name;
        txtComposer.text = songList[currentSong].composer;
        imageDisk.sprite = songList[currentSong].sprite;

        StartCoroutine(LoadAndPlaySequence()); // OSU 노래 데이터 파싱 로드 대기
    }
    
    IEnumerator LoadAndPlaySequence()
    {
        string songName = songList[currentSong].name;
        int previewTime = songList[currentSong].previewTime;

        // 1. StreamingAssets 폴더에서 Mp3 파일을 비동기로 들고 옴.
        yield return StartCoroutine(SoundManager.Instance.LoadBGMFromStreamingAssets(songName));

        // 2. 재생
        SoundManager.Instance.PlayBGM(songName, previewTime);
    }
    
    public void BtnBack()  // 버튼 이벤트 등록
    {
        SoundManager.Instance.StopBGM();
        
        TitleMenuUI.SetActive(true); // 타이틀 메뉴 활성화
        this.gameObject.SetActive(false); // 스테이지 비활
    }

    public void BtnPlay()  // 버튼 이벤트 등록
    {
        this.gameObject.SetActive(false); // 스테이지 비활
        
        GameManager.Instance.CurrentState = MusicState.Ready;
        
        SceneLoader sceneLoader = GameManager.Instance.GetSceneLoader();
        sceneLoader?.ChangeScene(1); // GameScene
        
        // GameManager에 Song에 대한 정보들을 저장함.
        GameManager.Instance.SetCurrentSong(songList[currentSong]);

        SoundManager.Instance.StopBGM();
        
        // 게임 Play 버튼 누르면 해당 곡의 JSON 파일 정보를 들고옴.
        DataManager.Instance.StartParsing(songList[currentSong].name);
    }
}