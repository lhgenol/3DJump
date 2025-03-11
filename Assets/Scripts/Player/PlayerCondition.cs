using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable    // IDamageable 인터페이스: 피해를 받을 수 있는 개체들이 반드시 구현해야 하는 메서드를 정의
{
    void TakePhysicalDamage(int damage);    // 물리 피해를 입는 기능
}

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;     // UI와 상태 데이터를 연동하는 객체
    
    // 현재 UICondition에서 관리하는 체력, 점프, 스태미나를 가져옴
    Condition jump { get {return uiCondition.jump;} }       // 점프
    Condition health { get {return uiCondition.health;} }   // 체력
    Condition stamina { get {return uiCondition.stamina;} } // 스태미나
    
    public event Action onTakeDamage;       // 피해를 입었을 때 실행될 이벤트
    
    void Update()
    {
        jump.Add(jump.passiveValue * Time.deltaTime);           // 점프 지속적으로 증가
        stamina.Add(stamina.passiveValue * Time.deltaTime);     // 스태미나 지속적으로 증가
        health.Add(health.passiveValue * Time.deltaTime);       // 체력 지속적으로 증가
        
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

    // IDamageable 인터페이스의 구현: 물리 피해를 입었을 때 실행되는 메서드
    public void TakePhysicalDamage(int damage)
    {
        health.Subtrack(damage);    // 체력을 감소시킴
        onTakeDamage?.Invoke();     // 피해를 입었음을 알리는 이벤트 호출
    }
}
