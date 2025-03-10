using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce; // 점프대가 주는 힘

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어가 점프대를 밟았을 때
        if (collision.gameObject.CompareTag("Player")) 
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
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
