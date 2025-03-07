using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition jump;      // 점프 UI
    public Condition health;    // 체력 UI
    public Condition stamina;   // 스태미나 UI
    
    void Start()
    {
        // 플레이어의 상태 UI를 현재 UICondition과 연결
        CharacterManager.Instance.Player.condition.uiCondition = this;  // 플레이어의 체력 UI를 설정
    }
}
