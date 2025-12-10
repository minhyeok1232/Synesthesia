using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject goUI = null;
    [SerializeField] Text[] txtCount = null;
    [SerializeField] Text txtScore = null;
    [SerializeField] Text txtMaxCombo = null;

    void Start()
    {
        ShowResult();
    }

    public void ShowResult()
    {
        TimingController timingController = GameManager.Instance.GetTimingController();
        ScoreController scoreController   = GameManager.Instance.GetScoreController();
        ComboController comboController   = GameManager.Instance.GetComboController();
        
        // 결과 UI 활상화
        goUI.SetActive(true);

        // 초기화
        for (int i = 0; i < txtCount.Length; i++)
            txtCount[i].text = "0";
        txtScore.text = "0";
        txtMaxCombo.text = "0";

        // 점수들 가져오기
        int[] t_judgement = timingController.GetJudgementRecord();
        int t_currentScore = scoreController.GetCurrentScore();
        int t_maxCombo = comboController.GetMaxCombo();

        // 텍스트 세팅 (가져온 것들로)
        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = string.Format("{0:#,##0}", t_judgement[i]);
        }
        txtScore.text = string.Format("{0:#,##0}", t_currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}", t_maxCombo);
    }
}