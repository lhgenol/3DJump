using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller; // 플레이어의 컨트롤러 참조
    public PlayerCondition condition;   // 플레이어의 컨디션 참조
   
    public ItemData itemData;           // ItemObject의 아이템 데이터 ItemDeta에다 넘겨주기(인터렉션 되면)
    public Action addItem;              // addItem이라는 델리게이트에 구독이 되어 있는 걸 실행시켜 주게 세팅

    public Transform dropPosition;
    
    public void Awake()
    {
        // 싱글톤 CharacterManager의 Player 속성에 현재 인스턴스를 설정
        CharacterManager.Instance.Player = this;
        
        // PlayerController 컴포넌트를 가져옴
        controller = GetComponent<PlayerController>();
        
        // PlayerCondition 컴포넌트를 가져옴
        condition = GetComponent<PlayerCondition>();
    }
}
