using UnityEngine;
using UnityEngine.UI;

public class ComboManager : Singleton<ComboManager>
{
    [SerializeField] private GameObject img_Combo = null;
    [SerializeField] private Text txt_Combo = null;

    private int currentCombo = 0;

    private void Start()
    {
        img_Combo.SetActive(false);
        txt_Combo.gameObject.SetActive(false);
    }
    
    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += p_num;
        txt_Combo.text = string.Format("{0:#,##}", currentCombo);

        if (currentCombo > 1)
        {
            img_Combo.SetActive(true);
            txt_Combo.gameObject.SetActive(true);
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        txt_Combo.text = "0";
        img_Combo.SetActive(false);
        txt_Combo.gameObject.SetActive(false);
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }
}
