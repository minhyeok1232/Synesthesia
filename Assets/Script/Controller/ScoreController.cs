using UnityEngine;
using UnityEngine.UI;


public class ScoreController : MonoBehaviour
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
        GameManager.Instance.SetScoreController(this);
        
        currentScore = 0;
        scoreText.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        ComboController comboController = GameManager.Instance.GetComboController();
        
        // 콤보 증가
        if (p_JudgementState != 3) 
            comboController.IncreaseCombo();
        
        // 콤보 보너스 계산
        int currentCombo = comboController.GetCurrentCombo();
        int scoreBonus = (currentCombo / 10) * comboBonusScore;
        
        // 점수 증가
        int t_increaseScore = increaseScore + scoreBonus;
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);
        
        // 점수 반영
        currentScore += t_increaseScore;
        scoreText.text = string.Format("{0:#,##0}", currentScore);
    }
    
    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    public void AnimPlayScore()
    {
        scoreAnimator.SetTrigger("ScoreUp");
    }
}
