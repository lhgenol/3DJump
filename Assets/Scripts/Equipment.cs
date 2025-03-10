using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;          // 현재 장착 중인 장비
    public Transform equipParent;   // 장비를 달아줄 위치 (카메라 위치)

    private PlayerController controller;    // 플레이어 컨트롤러 (입력 제어)
    private PlayerCondition condition;      // 플레이어 상태 (체력, 배고픔 등)
    
    void Start()
    {
        // 플레이어 컨트롤러 및 상태 컴포넌트 가져오기
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    // 새로운 장비를 장착하는 기능
    public void EquipNew(ItemData data)
    {
        // 기존에 장착 중인 장비가 있다면 먼저 해제
        UnEquip();
        // 새로운 장비 프리팹을 생성하고, 장착 위치에 배치
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();

    }

    // 현재 장비를 해제하는 기능
    public void UnEquip()
    {
        if (curEquip != null)   // 장착된 장비가 있다면 제거
        {
            Destroy(curEquip.gameObject);   // 현재 장비 오브젝트 삭제
            curEquip = null;    // 장착 정보 초기화
        }
    }
    
    // 공격 입력 처리 (플레이어가 공격 버튼을 눌렀을 때 실행됨)
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        // 공격 버튼이 눌려졌고, 장비가 장착되어 있으며, 인벤토리가 열려있지 않을 때만 실행
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput();   // 장착된 장비의 공격 기능 실행
        }
    }
}
