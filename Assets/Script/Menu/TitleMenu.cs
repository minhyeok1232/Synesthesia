using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] GameObject goStageUI = null;

    [Header("Buttons")] 
    [SerializeField] private Button btn_Play;

    private void Start()
    {
        if (btn_Play != null)
            btn_Play.onClick.AddListener(() => BtnPlay());
    }

    public void BtnPlay()  // 버튼 이벤트 등록
    {
        goStageUI.SetActive(true);  // 스테이지 메뉴 활성화
        this.gameObject.SetActive(false); // 타이틀 메뉴 끄고 
    }
}