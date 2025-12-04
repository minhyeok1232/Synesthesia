using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    [SerializeField] Animator scoreAnimator = null;
    
    [SerializeField] private Text scoreText;
    [SerializeField] private int increaseScore = 10;
    private int currentScore = 0;

    [SerializeField] private float[] weight = null;
    
    void Start()
    {
        scoreAnimator = GetComponent<Animator>();
        currentScore = 0;
        scoreText.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        int t_increaseScore = increaseScore;
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);
        currentScore += t_increaseScore;
        scoreText.text = string.Format("{0:#,##0}", currentScore);
    }
    
    public void animPlayScore()
    {
        scoreAnimator.SetTrigger("ScoreUp");
    }
}
