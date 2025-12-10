using UnityEngine;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    [SerializeField] GameObject TitleMenuUI = null;
    
    [Header("Buttons")] 
    [SerializeField] private Button btn_Back;
    [SerializeField] private Button btn_Play;
    
    private void Start()
    {
        if (btn_Back != null)
            btn_Back.onClick.AddListener(() => BtnBack());
        
        if (btn_Play != null)
            btn_Play.onClick.AddListener(() => BtnPlay());
    }
    
    public void BtnBack()  // 버튼 이벤트 등록
    {
        TitleMenuUI.SetActive(true); // 타이틀 메뉴 활성화
        this.gameObject.SetActive(false); // 스테이지 비활
    }

    public void BtnPlay()  // 버튼 이벤트 등록
    {
        this.gameObject.SetActive(false); // 스테이지 비활
        
        SceneLoader sceneLoader = GameManager.Instance.GetSceneLoader();
        sceneLoader?.ChangeScene(1); // GameScene
    }
}