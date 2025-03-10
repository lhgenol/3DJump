using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;     // UI와 상태 데이터를 연동하는 객체
    
    // 현재 UICondition에서 관리하는 체력, 점프, 스태미나를 가져옴
    Condition jump { get {return uiCondition.jump;} }       // 점프
    Condition health { get {return uiCondition.health;} }   // 체력
    Condition stamina { get {return uiCondition.stamina;} } // 스태미나
    
    void Update()
    {
        jump.Add(jump.passiveValue * Time.deltaTime);           // 점프 지속적으로 증가
        stamina.Add(stamina.passiveValue * Time.deltaTime);     // 스태미나 지속적으로 증가
        
        if (health.curValue == 0f)      // 체력이 0이 되면
        {
            Die();      // 사망 처리
        }
    }
    
    // 플레이어 사망 처리
    public void Die()
    {
        Debug.Log("죽었다!");
    }
    
    // 체력 회복 기능
    public void Heal(float amount)
    {
        health.Add(amount);
    }
}
