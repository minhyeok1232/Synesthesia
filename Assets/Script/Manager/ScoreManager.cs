using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : Singleton<ScoreManager>
{
    // Animator
    [SerializeField] Animator scoreAnimator = null;
    
    // Score
    [SerializeField] private Text scoreText;
    [SerializeField] private int increaseScore = 10;
    private int currentScore = 0;

    [SerializeField] private float[] weight = null;
    [SerializeField] private int comboBonusScore = 10;
    
    void Start()
    {
        currentScore = 0;
        scoreText.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        ComboManager comboManager = GameManager.Instance.GetComboManager();
        
        // 콤보 증가
        comboManager.IncreaseCombo();
        
        // 콤보 보너스 계산
        int currentCombo = comboManager.GetCurrentCombo();
        int scoreBonus = (currentCombo / 10) * comboBonusScore;
        
        // 점수 증가
        int t_increaseScore = increaseScore + scoreBonus;
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);
        
        // 점수 반영
        currentScore += t_increaseScore;
        scoreText.text = string.Format("{0:#,##0}", currentScore);
        
        
    }
    
    public void AnimPlayScore()
    {
        scoreAnimator.SetTrigger("ScoreUp");
    }
}
