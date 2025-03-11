using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IInteractable
{
    public ItemData data;
    
    public float speed;    // 이동 속도
    public float distance; // 이동 범위

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = false; // 플레이어와 충돌했을 때만 움직이도록

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.right * distance; // 오른쪽으로 이동
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.PingPong(Time.time * speed, 1));

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Vector3 temp = startPosition;
                startPosition = targetPosition;
                targetPosition = temp;
            }
        }
    }
    
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isMoving = true;
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지됨! (발판에서 내려감)");
            isMoving = false;
            collision.transform.SetParent(null);
        }
    }
    
    public string GetInteractPrompt()   // 이러면 어떤 아이템이든 간에 IInteractable이라고 하는 컴포넌트를 찾고, 없다면 넘어가고 있다면 그 안의 기능들을 쓸 수 있음
    {
        // 프롬프트에 띄워줄 정보
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        
    }
}