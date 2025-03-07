using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;      // 현재 상태 값
    public float startValue;    // 게임 시작 시 초기 값
    public float maxValue;      // 상태 최대 값 (최대 체력)
    public float passiveValue;  // 주기적으로 변하는 값 (예: 배고픔 감소량)
    public Image uiBar;         // UI 바 (체력, 배고픔 바 등)
    
    void Start()
    {
        curValue = startValue;  // 시작 시 상태 값을 초기 값으로 설정
    }
    
    void Update()
    {
        uiBar.fillAmount = GetPercentage(); // UI 바 업데이트 (현재 상태에 맞춰 크기 조정)
    }
    
    // 현재 값이 전체 값에서 몇 %인지 계산해 반환하는 함수
    float GetPercentage()
    {
        return curValue / maxValue;       // 현재 값을 초기 값으로 나눈 백분율
    }
    
    // 상태 값을 추가하는 함수 (체력 회복 등)
    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);   // curValue에 값을 더하지만, maxValue를 초과하지 않도록 제한
    }

    // 상태 값을 감소시키는 함수 (데미지, 배고픔 감소 등)
    public void Subtrack(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);  // curValue에서 값을 빼지만, 0 미만으로 내려가지 않도록 제한
    }  
}
