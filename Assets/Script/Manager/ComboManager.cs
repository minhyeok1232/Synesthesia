using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private GameObject goComboImage = null;
    [SerializeField] private Text txtCombo = null;

    private int currentCombo = 0;

    private void Start()
    {
        goComboImage.SetActive(false);
        txtCombo.gameObject.SetActive(false);
    }
    
    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += p_num;
        txtCombo.text = string.Format("{0:#,##}", currentCombo);

        if (currentCombo > 1)
        {
            goComboImage.SetActive(true);
            txtCombo.gameObject.SetActive(true);
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        txtCombo.text = "0";
        goComboImage.SetActive(false);
        txtCombo.gameObject.SetActive(false);
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }
}
