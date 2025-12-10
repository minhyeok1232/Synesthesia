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
    
    
    [SerializeField] private AudioManager  audioManager;
    
    [Header("Scenes")]
    [SerializeField] private SceneLoader   sceneLoader;
    
    void Awake()
    {
        EnsureSingleInstance();
    }
    
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

    public AudioManager GetAudioManager()
    {
        return audioManager;
    }

    public SceneLoader GetSceneLoader()
    {
        return sceneLoader;
    }
    
    #endregion
}
