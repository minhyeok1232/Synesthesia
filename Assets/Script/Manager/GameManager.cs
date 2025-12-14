using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Controllers")]
    private ComboController  comboController;
    private EffectController effectController;
    private NoteController   noteController;
    private PlayerController playerController;
    private ScoreController  scoreController;
    private TimingController timingController;
    
    [Header("Scenes")]
    [SerializeField] private SceneLoader   sceneLoader;

    private Song currentSong = null;
    
    void Awake()
    {
        EnsureSingleInstance();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
            Initialized();
    }
    
    
    #region Initialized

    void Initialized()
    {
        // 게임 시작 시 진입
        // MusicStart -> false
        CenterFlame.musicStart = false;
        
        // Score, Combo, Timing -> 초기화
        scoreController.Initialized();
        comboController.ResetCombo();
        timingController.Initialized();
        
        //TODO 
        //TimingManager에 있는 boxNoteList 를 Clear해주는 작업도 필요함
        
        
    }
    
    #endregion
    
    #region Get & Set
    
    public void SetComboController(ComboController _controller)
    {
        comboController = _controller;
    }
    public void SetEffectController(EffectController _controller)
    {
        effectController = _controller;
    }
    public void SetNoteController(NoteController _controller)
    {
        noteController = _controller;
    }
    public void SetPlayerController(PlayerController _controller)
    {
        playerController = _controller;
    }
    public void SetScoreController(ScoreController _controller)
    {
        scoreController = _controller;
    }
    public void SetTimingController(TimingController _controller)
    {
        timingController = _controller;
    }
    public void SetCurrentSong(Song song)
    {
        currentSong = song;
    }
    
    public ComboController GetComboController()
    {
        return comboController;
    }
    public EffectController GetEffectController()
    {
        return effectController;
    }
    public NoteController GetNoteController()
    {
        return noteController;
    }
    public ScoreController GetScoreController()
    {
        return scoreController;
    }
    public TimingController GetTimingController()
    {
        return timingController;
    }

    public SceneLoader GetSceneLoader()
    {
        return sceneLoader;
    }

    public Song GetCurrentSong()
    {
        return currentSong;
    }
    
    #endregion
}
