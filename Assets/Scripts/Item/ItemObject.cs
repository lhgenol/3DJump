using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();  // 화면에 띄워 줄 프롬프트
    public void OnInteract();           // 인터렉트 됐을 때 어떤 효과를 발생시킬 건지
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()   // 이러면 어떤 아이템이든 간에 IInteractable이라고 하는 컴포넌트를 찾고, 없다면 넘어가고 있다면 그 안의 기능들을 쓸 수 있음
    {
        // 프롬프트에 띄워줄 정보
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;   // ItemDeta에다 내가 갖고 있는 데이터 넣어주기
        CharacterManager.Instance.Player.addItem?.Invoke(); // 이 addItem에 필요한 기능을 구독시켜 놓으면 됨
        Destroy(gameObject);    // 인벤토리로 이동 후 맵 위의 아이템은 제거
    }
}
