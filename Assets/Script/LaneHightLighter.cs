using UnityEngine;
using UnityEngine.UI;

public class LaneHighlighter : MonoBehaviour
{
    private Image hitImage;
    public float fadeSpeed = 5f; // 사라지는 속도 (취향에 맞게 조절)
    private bool isHolding = false; // 현재 누르고 있는 상태인지 체크

    void Awake()
    {
        hitImage = GetComponent<Image>();
    }

    void Update()
    {
        // 꾹 누르고 있는 상태가 아닐 때만 알파값을 줄임
        if (!isHolding && hitImage.color.a > 0)
        {
            Color newColor = hitImage.color;
            newColor.a -= fadeSpeed * Time.deltaTime;
            hitImage.color = newColor;
        }
    }

    // 키를 처음 누를 때 호출 (이미지를 팍 밝게 만듦)
    public void OnPress()
    {
        isHolding = true; // 감쇠 중단
        Color newColor = hitImage.color;
        newColor.a = 1f; 
        hitImage.color = newColor;
    }

    // 키를 뗄 때 호출 (이때부터 Update에서 서서히 사라짐)
    public void OnRelease()
    {
        isHolding = false; // 감쇠 시작
    }
}