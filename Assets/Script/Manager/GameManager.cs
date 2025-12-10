using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Managers")]
    [SerializeField] private ComboManager  comboManager;
    [SerializeField] private EffectManager effectManager;
    [SerializeField] private NoteManager   noteManager;
    [SerializeField] private ScoreManager  scoreManager;
    [SerializeField] private TimingManager timingManager;
    [SerializeField] private AudioManager  audioManager;
    
    [Header("Scenes")]
    [SerializeField] private SceneLoader   sceneLoader;
    
    void Awake()
    {
        EnsureSingleInstance();
    }
    
    #region Get
    
    public ComboManager GetComboManager()
    {
        return comboManager;
    }
    public EffectManager GetEffectManager()
    {
        return effectManager;
    }
    public NoteManager GetNoteManager()
    {
        return noteManager;
    }
    public ScoreManager GetScoreManager()
    {
        return scoreManager;
    }
    public TimingManager GetTimingManager()
    {
        return timingManager;
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
