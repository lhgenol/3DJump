using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;    // 인벤토리 슬롯 배열. 각 슬롯에 들어가는 아이템을 관리
    
    public GameObject inventoryWindow;  // 인벤토리 창 (UI)
    public Transform slotPanel;         // 인벤토리 슬롯들이 위치한 패널
    public Transform dropPosition;      // 아이템을 버릴 때 아이템이 생성될 위치
    
    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;        // 선택한 아이템의 이름
    public TextMeshProUGUI selectedItemDescription; // 선택한 아이템의 설명
    public TextMeshProUGUI selectedStatName;        // 선택한 아이템의 능력치 종류
    public TextMeshProUGUI selectedStatValue;       // 선택한 아이템의 능력치 값
    public GameObject useButton;                // 사용 버튼
    public GameObject equipButton;              // 장착 버튼
    public GameObject unequipButton;            // 해제 버튼
    public GameObject dropButton;               // 버리기 버튼
    
    private PlayerController controller;        // 플레이어의 컨트롤러 (조작 관련)
    private PlayerCondition condition;          // 플레이어의 상태 (체력, 배고픔 등)
    
    ItemData selectedItem;          // 현재 선택된 아이템 정보
    int selectedItemIndex = 0;      // 선택한 아이템의 슬롯 인덱스

    int curEquipIndex;
    
    void Start()
    {
        // 플레이어 관련 정보를 가져옴
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;
        
        // 인벤토리 열기/닫기 기능을 플레이어 입력 이벤트에 연결
        controller.inventory += Toggle;     
        
        // 아이템 추가 기능을 플레이어의 이벤트에 연결
        CharacterManager.Instance.Player.addItem += AddItem;   
        
        // 인벤토리 창은 기본적으로 비활성화
        inventoryWindow.SetActive(false); 
        
        // UI에 있는 슬롯 개수만큼 배열 초기화. slotPanel이라는 Transform 기준으로 자식들이 몇 개가 있는지 갖고 올 수 있음
        slots = new ItemSlot[slotPanel.childCount];
        
        // 슬롯 패널에서 슬롯들을 찾아서 배열에 저장
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();  // 각 슬롯을 가져옴
            slots[i].index = i;         // 슬롯의 인덱스 설정
            slots[i].inventory = this;  // 각 슬롯이 인벤토리를 참조하도록 설정
        }

        CleatSelectedItemWindow();      // 선택된 아이템 창 초기화
    }

    // 선택된 아이템 창을 초기화
    void CleatSelectedItemWindow()
    {
        // 선택된 아이템 창 초기화
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;
        
        // 모든 버튼 비활성화
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }
    
    // 인벤토리 창을 열거나 닫는 기능
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true); 
        }
    }
    
    // 인벤토리 창이 하이어라키 창에 활성화되어 있는지 확인
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;  
    }
    
    void AddItem()
    {
        // 플레이어가 아이템을 획득했을 때 호출됨
        ItemData data = CharacterManager.Instance.Player.itemData;  // 아이템 데이터 가져오기
        
        // 아이템이 중첩 가능하다면 기존 슬롯을 찾아 추가 (canStack 체크)
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            
            // 슬롯이 비어있지 않다면
            if (slot != null)
            {
                slot.quantity++;    // 아이템의 개수만 증가. 슬롯의 양 늘려주기
                UpdateUI();         // UI 갱신
                CharacterManager.Instance.Player.itemData = null;   // 아이템 데이터 초기화
                return;
            }
        }
        
        // 빈 슬롯을 찾아 아이템을 추가
        ItemSlot emptySlot = GetEnptySlot();
        
        // 비어있는 슬롯이 있다면
        if (emptySlot != null)
        {
            emptySlot.item = data;  // 데이터 넣어주기
            emptySlot.quantity = 1; // 양 올려주기
            UpdateUI();             // UI 갱신
            CharacterManager.Instance.Player.itemData = null;   // 아이템 데이터 초기화
            return;
        }
        
        // 빈 슬롯이 없다면 아이템을 버림
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;   // 아이템 데이터 초기화
    }
    
    // 같은 아이템이 있는 슬롯을 찾음 (중첩 가능할 때)
    ItemSlot GetItemStack(ItemData data)
    {
        // 이미 있는 아이템이면 해당 슬롯을 반환
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯 아이템이 넣으려는 데이터가 같다면, 그리고 아이템의 양이 최대 양보다 작다면
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];    // 해당되는 슬롯을 반환
            }
        }
        return null;    // 아니면 null 반환
    }
    
    // UI를 갱신 (각 슬롯 업데이트)
    void UpdateUI()
    {
        // 배열에 있는 걸 순회. 슬롯 UI 업데이트 (비어있는 슬롯과 채워진 슬롯 구분)
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)   // 데이터가 들어가 있다면
            {
                slots[i].Set(); // 세팅해줘라
            }
            else // 아니면
            {
                slots[i].Clear();   // 슬롯 자체에서 비어있을 때 UI 세팅하는 로직 실행
            }
        }
    }
    
    // 비어있는 슬롯을 찾음. 아이템 중복이 가능한 게 아니라면 비어있는 슬롯을 가져옴
    ItemSlot GetEnptySlot()
    {
        // 비어있는 슬롯을 찾아 반환
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯 아이템이 비어있다면 
            if (slots[i].item == null)
            {
                return slots[i];    // null인 슬롯을 반환
            }
        }
        return null;    // 꽉 차있다면 null 반환
    }
    
    // 아이템을 버리는 기능
    void ThrowItem(ItemData data)
    {
        // 아이템을 현재 위치에 버리는 기능
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }
    
    // 특정 슬롯의 아이템을 선택
    public void SelectItem(int index)
    {
        // 특정 슬롯을 클릭하면 해당 아이템을 선택. 슬롯이 비어 있으면 아무것도 선택하지 않음
        if(slots[index].item == null) return; 
        
        // 선택한 아이템을 저장
        selectedItem = slots[index].item;
        selectedItemIndex = index;
    
        // UI에 아이템의 이름과 설명 표시
        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;
        
        // 아이템의 능력치 정보를 초기화 (기존 정보 삭제)
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // 아이템이 소비형(Consumable)일 경우, 해당 아이템의 능력치 정보를 UI에 표시
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            // 능력치 타입 정보 표시 (예: 체력 회복, 배고픔 감소 등)
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            
            // 해당 능력치의 값 표시 (얼마만큼 영향을 주는지)
            selectedStatValue.text+= selectedItem.consumables[i].value.ToString() + "\n";
        }
        
        // 버튼 활성화 조건 설정
        useButton.SetActive(selectedItem.type == ItemType.Consumable);  // 소비형 아이템일 경우 '사용' 버튼 활성화
        // 장착 가능한 아이템이고, 현재 장착 중이 아닐 경우 '장착' 버튼 활성화
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);   
        // 장착 가능한 아이템이고, 현재 장착 중이면 '해제' 버튼 활성화
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable  && slots[index].equipped);
        
        dropButton.SetActive(true); // 모든 아이템은 버릴 수 있으므로 '버리기' 버튼 활성화
    }
    
    // 선택한 아이템을 사용할 때 호출되는 메서드
    public void OnUseButton()
    {
        // 선택된 아이템이 소비형(Consumable)인지 확인
        if (selectedItem.type == ItemType.Consumable)
        {
            // 소비형 아이템의 효과를 적용
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConsumableType.SpeedUp:
                        StartCoroutine(SpeedBuff(selectedItem.consumables[i].value, 5f)); // 이동 속도 증가를 5초 지속
                        break;
                    case ConsumableType.JumpUp:
                        StartCoroutine(JumpBuff(selectedItem.consumables[i].value, 5f)); // 점프 파워 증가를 5초 지속
                        break;
                }
            }
            RemoveSelectedItem();   // 사용한 아이템을 인벤토리에서 제거
        }
    }
    
    // 이동 속도 증가 효과 (일정 시간 후 원래 속도로 복구)
    IEnumerator SpeedBuff(float amount, float duration)
    {
        controller.moveSpeed += amount;
        yield return new WaitForSeconds(duration);
        controller.moveSpeed -= amount;
    }
    
    // 점프 파워 증가 효과 (일정 시간 후 원래 점프력 파워로 복구)
    IEnumerator JumpBuff(float amount, float duration)
    {
        controller.jumpPower += amount;
        yield return new WaitForSeconds(duration);
        controller.jumpPower -= amount;
    }
    
    public void OnDropButton()
    {
        // "버리기" 버튼 클릭 시
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }
    
    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;    // 선택한 아이템을 제거 (개수가 0이면 삭제). 선택한 아이템의 개수를 1 감소시킴

        // 아이템 개수가 0 이하가 되면 슬롯에서 완전히 제거
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;                    // 선택된 아이템 초기화
            slots[selectedItemIndex].item = null;   // 슬롯에서 아이템 제거
            selectedItemIndex = -1;                 // 선택한 슬롯 인덱스 초기화
            CleatSelectedItemWindow();              // UI에서 선택된 아이템 창을 비움
        }
    
        UpdateUI(); // 인벤토리 UI 갱신
    }
}
