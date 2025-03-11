using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 유형을 정의하는 열거형 
public enum ItemType
{
    Equipable,  // 장비형 아이템 (무기, 방어구 등)
    Consumable, // 소비형 아이템 (포션, 음식 등)
    Resource    // 자원형 아이템 (나무, 광석 등)
}

// 소비형 아이템의 효과 유형을 정의하는 열거형
public enum ConsumableType  
{
    SpeedUp,    // 스피드를 올려주는 아이템
    JumpUp      // 점프력을 올려주는 아이템
}

// 소비형 아이템의 효과 데이터를 저장하는 클래스
[Serializable]  // 인스펙터에서 보이도록 설정
public class ItemDataConsumable
{
    public ConsumableType type; // 아이템이 스피드/점프력 중 어떤 효과를 가지는지 결정
    public float value;         // 얼마만큼 회복시켜 줄지 값
}

// 아이템 데이터 (ScriptableObject)
[CreateAssetMenu(fileName = "Item", menuName = "New Item")] // 인스펙터에서 ScriptableObject로 생성 가능하게
public class ItemData : ScriptableObject
{
    [Header("Info")]   
    public string displayName;  // 아이템 이름
    public string description;  // 아이템 설명
    public ItemType type;       // 아이템 유형 (장비, 소비, 자원)
    public Sprite icon;         // 아이템 아이콘 (UI에서 사용)
    public GameObject dropPrefab;   // 아이템이 바닥에 떨어질 때 나타나는 프리팹
    
    [Header("Stacking")]
    public bool canStack;      // 여러 개 가질 수 있는 아이템인지 체크 (ex. 포션은 가능, 검은 불가능)
    public int maxStackAmount; // 한 슬롯에 최대 몇 개까지 쌓을 수 있는지
    
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;    // 소비형 아이템이 여러 개의 효과를 가질 수 있도록 배열로 저장 (ex. 스피드 + 점프력)
    
    [Header("Equip")]
    public GameObject equipPrefab;  // 장착 아이템 프리팹 정보
}
