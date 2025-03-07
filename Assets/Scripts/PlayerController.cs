using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;             // 이동 속도
    public float jumpPower;             // 점프 힘
    private Vector2 curMovementInput;   // 현재 이동 입력 값
    public LayerMask groundLayerMask;   // 바닥 체크를 위한 레이어 마스크
    
    [Header("Look")]
    public Transform cameraContainer;   // 카메라 컨테이너
    public float minXLook;              // 카메라의 최소 X 회전 값
    public float maxXLook;              // 카메라의 최대 X 회전 값
    private float camCurXrot;           // 현재 카메라 X 회전 값. 마우스의 델타 값을 받는 변수
    public float lookSensitivity;       // 마우스 감도. 회전 민감도
    private Vector2 mouseDelta;         // 마우스 이동 값
    public bool canLook = true;         // 인벤토리를 켰을 땐 커서가 보여야 함
    
    //public Action inventory;
    private Rigidbody _rigidbody;       // 플레이어의 Rigidbody
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 시작하면 커서는 락 모드. 마우스 커서를 잠가서 화면 중앙에 고정
    }
    
    void FixedUpdate()
    {
        Move();         // 이동 처리
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();   // true일 때만 카메라가 돌아갈 수 있게 설정
        }
        CameraLook();   // 카메라 조작 처리
    }
    
    // 플레이어 이동 처리
    void Move()
    {
        // 입력값을 기준으로 이동 방향 계산
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;               // 이동 속도 곱해주기
        dir.y = _rigidbody.velocity.y;  // 기존 Y축 속도 유지 (중력 반영)
        _rigidbody.velocity = dir;      // Rigidbody의 방향, 속도 설정
    }
    
    // 이동 입력 처리 (WASD 키 등)
    public void OnMove(InputAction.CallbackContext context) // context는 현재 상태를 받아올 수가 있음
    {
        // 키가 계속 눌리는 상태라면
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();    // 이동
        }
        // 키가 눌렸다가 떨어졌을 때
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;    // 정지
        }
    }
    
    // 카메라 회전 처리
    void CameraLook()
    {
        // 마우스 움직임을 반영해 X축(상하) 회전 값 변경
        camCurXrot += mouseDelta.y * lookSensitivity;
        
        // camCurXrot 값을 minXLook ~ maxXLook 범위로 제한
        camCurXrot = Mathf.Clamp(camCurXrot, minXLook, maxXLook);
        
        // 카메라 컨테이너 회전 적용
        cameraContainer.localEulerAngles = new Vector3(-camCurXrot, 0, 0);
        
        // Y축(좌우) 회전 적용
        transform.localEulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    
    // 마우스 입력 처리 (카메라 회전)
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    
    // 점프 입력 처리
    public void OnJump(InputAction.CallbackContext context)
    {
        // 키를 눌렀고 플레이어가 바닥에 있는 상태라면
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            // 위 방향으로 순간적인 힘을 가함. 순간적으로 힘을 줄 수 있게 Impulse로 설정
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }
    
    // 플레이어가 바닥에 있는지 확인하는 함수
    bool isGrounded()
    {
        Ray[] rays = new Ray[4]     // 네 개의 Ray를 사용하여 플레이어가 바닥에 닿았는지 확인
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // 각 레이캐스트가 바닥에 닿았는지 검사
        for (int i = 0; i < rays.Length; i++)
        {
            if(Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;    // 하나라도 닿으면 바닥에 있는 것으로 판단하고 true 반환
            }
        }
        
        return false;   // 모든 레이가 바닥에 닿지 않으면 false 반환
    }
}
