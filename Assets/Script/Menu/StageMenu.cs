using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Song
{
    public string name;
    public string composer;
    public int bpm;
    public Sprite sprite;
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

    private int currentSong = 0;

    public void NextSong()
    {
        SoundManager.Instance.PlaySFX("Touch");
        
        if (++currentSong > songList.Length - 1)
            currentSong = 0;
        SettingSong();
    }

    public void PriorSong()
    {
        SoundManager.Instance.PlaySFX("Touch");
        
        if (--currentSong < 0)
            currentSong = songList.Length - 1;
        SettingSong();
    }

    private void SettingSong()
    {
        txtSongName.text = songList[currentSong].name;
        txtComposer.text = songList[currentSong].composer;
        imageDisk.sprite = songList[currentSong].sprite;

        SoundManager.Instance.PlayBGM("BGM" + currentSong);
    }
    
    public void BtnBack()  // 버튼 이벤트 등록
    {
        TitleMenuUI.SetActive(true); // 타이틀 메뉴 활성화
        this.gameObject.SetActive(false); // 스테이지 비활
    }

    public void BtnPlay()  // 버튼 이벤트 등록
    {
        this.gameObject.SetActive(false); // 스테이지 비활
        
        SceneLoader sceneLoader = GameManager.Instance.GetSceneLoader();
        sceneLoader?.ChangeScene(1); // GameScene
        
        // GameManager에 Song에 대한 정보들을 저장함.
        GameManager.Instance.SetCurrentSong(songList[currentSong]);

        SoundManager.Instance.StopBGM();
    }
}