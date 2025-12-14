using UnityEngine;
using UnityEngine.UI;

public class ComboController : MonoBehaviour
{
    // Animator
    [SerializeField] Animator comboAnimator = null;
    
    [SerializeField] private Text txt_Combo = null;
    [SerializeField] private Text txt_Combo_Score = null;

    private int currentCombo = 0;
    private int maxCombo = 0;

    void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetComboController(this);
    }
    
    private void Start()
    {
        txt_Combo.gameObject.SetActive(false);
        txt_Combo_Score.gameObject.SetActive(false);
    }
    
    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += p_num;
        txt_Combo_Score.text = string.Format("{0:#,##}", currentCombo);

        if (currentCombo > 1)
        {
            txt_Combo.gameObject.SetActive(true);
            txt_Combo_Score.gameObject.SetActive(true);
            AnimComboUp();
        }
        
        if (maxCombo < currentCombo)
            maxCombo = currentCombo;
    }

    public void AnimComboUp()
    {
        comboAnimator.SetTrigger("ComboUp");
    }
    
    public void ResetCombo()
    {
        currentCombo = 0;
        txt_Combo_Score.text = "0";
        txt_Combo.gameObject.SetActive(false);
        txt_Combo_Score.gameObject.SetActive(false);
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }
    
    public int GetMaxCombo()
    {
        return maxCombo;
    }
}
