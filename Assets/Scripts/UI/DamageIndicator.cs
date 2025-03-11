using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;         // 피해 시 깜빡이는 UI 이미지
    public float flashSpeed;    // 페이드아웃 속도
    
    private Coroutine coroutine;    // 현재 실행 중인 코루틴을 저장할 변수
    
    void Start()
    {
        // 피해 발생 시 Flash() 실행. onTakeDamage에 Flash 더해주기
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;  
    }

    public void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);   // 기존 코루틴 중지 (중복 실행 방지)
        }
        
        image.enabled = true;           // 이미지 활성화
        image.color = new Color(1f, 100f / 255f, 100 / 255f);   // 빨간색 설정
        coroutine = StartCoroutine(FadeAway());     // 페이드아웃 실행
    }
    
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;    // 알파값 감소
            image.color = new Color(1f, 100f / 255f, 100 / 255f, a);
            yield return null;  // 반환값 정해주기
        }
        image.enabled = false;  // 이미지 숨기기
    }
}
