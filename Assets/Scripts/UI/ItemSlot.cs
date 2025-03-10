using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;                   // 슬롯에 들어갈 아이템에 대한 정보

    public Button button;                   // 슬롯 클릭 버튼
    public Image icon;                      // 아이템 아이콘 이미지
    public TextMeshProUGUI quantityText;    // 아이템 개수를 표시하는 텍스트
    public Outline outline;                 // 슬롯이 선택됐을 때 표시되는 아웃라인
    
    public UIInventory inventory;           // 이 슬롯이 속한 인벤토리 UI
    
    public int index;       // 슬롯의 인덱스 (몇 번째 아이템 슬롯인지 구별)
    public bool equipped;   // 아이템이 장착되었는지 여부
    public int quantity;    // 해당 슬롯에 있는 아이템 개수
    
    private void Awake()
    {
        outline = GetComponent<Outline>();  // Outline 컴포넌트를 가져와서 저장
    }
    
    private void OnEnable()
    {
        outline.enabled = equipped; // 슬롯이 활성화될 때 장착된 아이템이라면 아웃라인을 활성화
    }
    
    // 슬롯 UI를 설정하는 메서드
    public void Set()
    {
        icon.gameObject.SetActive(true);    // 흰색 배경. 아이콘을 활성화하고 
        icon.sprite = item.icon;    // 해당 아이템의 스프라이트를 적용
        
        // 아이템 개수가 1보다 크다면 개수를 표시, 아니면 빈 문자열
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        // 아웃라인이 존재한다면, 장착 여부에 따라 활성화
        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    // 슬롯을 초기화하는 메서드
    public void Clear()
    {
        // 아이템 정보를 제거하고, 아이콘과 개수 텍스트를 숨김
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }
    
    // 슬롯 클릭 시 실행되는 메서드
    public void OnClickButton()
    {
        inventory.SelectItem(index);    // 나 자신의 인덱스 넘겨주기. 현재 슬롯의 인덱스를 인벤토리에 전달하여 선택된 아이템으로 설정
    }   
}
