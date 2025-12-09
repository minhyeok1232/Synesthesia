using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    [SerializeField] Animator scoreAnimator = null;
    
    [SerializeField] private Text scoreText;
    [SerializeField] private int increaseScore = 10;
    private int currentScore = 0;

    [SerializeField] private float[] weight = null;
    [SerializeField] private int comboBonusScore = 10;
    
    private ComboManager theCombo;
    
    void Start()
    {
        theCombo = FindObjectOfType<ComboManager>();
        scoreAnimator = GetComponent<Animator>();
        currentScore = 0;
        scoreText.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        // 콤보 증가
        theCombo.IncreaseCombo();
        
        // 콤보 보너스 계산
        int currentCombo = theCombo.GetCurrentCombo();
        int scoreBonus = (currentCombo / 10) * comboBonusScore;
        
        // 점수 증가
        int t_increaseScore = increaseScore + scoreBonus;
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);
        
        // 점수 반영
        currentScore += t_increaseScore;
        scoreText.text = string.Format("{0:#,##0}", currentScore);
        
        
    }
    
    public void animPlayScore()
    {
        scoreAnimator.SetTrigger("ScoreUp");
    }
}
