using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 점프대 스크립트
public class JumpPad : MonoBehaviour, IInteractable
{
    public ItemData data;   // JumpPad 데이터 담을 곳
    
    public float jumpForce; // 점프대가 플레이어에게 가하는 힘
    
    public string GetInteractPrompt()   // 이러면 어떤 아이템이든 간에 IInteractable이라고 하는 컴포넌트를 찾고, 없다면 넘어가고 있다면 그 안의 기능들을 쓸 수 있음
    {
        // 프롬프트에 띄워줄 정보
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        
    }

    // 충돌했을 때 실행되는 함수
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 "Player"인지 확인
        if (collision.gameObject.CompareTag("Player")) 
        {
            // 플레이어 오브젝트에서 Rigidbody 컴포넌트를 가져옴
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            // Rigidbody가 존재하는지 확인
            if (playerRb != null)
            {
                // 플레이어에게 점프 힘을 가함 (위쪽 방향으로 jumpForce만큼 힘을 가함)
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                // 플레이어 점프 횟수 초기화 (중복 점프 방지)
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.ResetJumpCount();
                }
            }
        }
    }
}
