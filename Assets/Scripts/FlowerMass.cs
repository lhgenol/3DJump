using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerMass : MonoBehaviour, IInteractable
{
    public ItemData data;
    
    public int damage; // 불에 닿으면 받을 피해량
    public float damageRate; // 피해를 주는 간격(초 단위)

    List<IDamageable> things = new List<IDamageable>(); // 피해를 받을 수 있는 객체 리스트 

    void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate); // 일정 간격마다 DealDamage() 호출
    }

    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage); // 리스트에 있는 모든 객체에 피해 적용
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable)) // 충돌한 객체가 IDamageable을 구현하고 있다면
        {
            things.Add(damageable); // 리스트에 추가하여 지속적인 피해 적용 (IDamageable을 구현한 객체만 리스트에 추가)
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable)) // 충돌이 끝난 객체를 리스트에서 제거
        {
            things.Remove(damageable); // 범위를 벗어나면 리스트에서 제거
        }
    }

    public string GetInteractPrompt() // 이러면 어떤 아이템이든 간에 IInteractable이라고 하는 컴포넌트를 찾고, 없다면 넘어가고 있다면 그 안의 기능들을 쓸 수 있음
    {
        // 프롬프트에 띄워줄 정보
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {

    }
}
