using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 얼마나 자주 Ray를 쏴서 상호작용 가능한 오브젝트를 감지할지 (초 단위)
    private float lastCheckTime;    // 마지막으로 Ray를 체크한 시간
    public float maxCheckDistance;  // 최대 감지 거리 (플레이어가 얼마나 멀리 있는 오브젝트까지 감지할 수 있는지)
    public LayerMask layerMask;     // 감지할 오브젝트가 포함된 레이어 지정. 어떤 레이어가 달려 있는 게임 오브젝트를 추출할 건지
    
    public GameObject curInteractGameObject;    // 현재 상호작용 가능한 오브젝트 (감지된 오브젝트)
    private IInteractable curInteractable;      // 현재 감지된 오브젝트의 인터페이스 (상호작용을 위한 인터페이스)
    
    public TextMeshProUGUI promptText;  // 화면에 상호작용 프롬프트(메시지)를 출력할 UI 텍스트
    private Camera camera;              // 플레이어가 바라보는 메인 카메라
    
    void Start()
    {
        camera = Camera.main;   // 현재 씬에서 MainCamera를 가져옴
        
        if (promptText == null)
        {
            Debug.LogError("promptText가 설정되지 않았습니다! UI 오브젝트를 연결하세요.");
        }
    }
    
    void Update()
    {
        // 얼마나 자주 감지할지 정하는 로직. 현재 시간에서 마지막 체크 시간을 뺀 값이 checkRate(0.05)보다 크면 감지 실행
        // 만약 현재 시간이 1초, lastCheckTime이 0초라고 가정하면 1 - 0 = 1. 0.05(checkRate)보다 훨씬 큼.
        // -> lastCheckTime의 현재 시간이 1이니까 1이 들어가고 아래 로직이 쭉 진행된 다음에 현재 시간이 1에서 1.02가 되면 1.02 - 1 = 0.02. 0.05(checkRate)보다 작음.
        // -> 이제 if문 안에 로직을 안 탐. 다시 checkTime이 0.06이 되면 lastCheckTime의 1을 빼 줘서 checkRate의 0.05보다 크니까 안에 로직이 다시 수행됨
        if (Time.time - lastCheckTime > checkRate) 
        {
            lastCheckTime = Time.time;  // lastCheckTime을 현재 시간으로 업데이트
            
            // 화면 중앙에서 Ray(광선)를 발사하여 감지할 오브젝트를 찾음
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));    // origin
            RaycastHit hit;     // Ray가 충돌한 오브젝트의 정보를 담아 놓을 변수
        
            // Ray 쏘기 (ray 정보 담기, 충돌된 물체가 있다면 hit에 정보 넘겨주기, 길이, 레이어마스크)
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // 새로운 오브젝트를 감지한 경우 (이미 감지한 게임 오브젝트가 없을 때 (혹은 같지 않을 때))
                if(hit.collider.gameObject != curInteractGameObject)   
                {
                    curInteractGameObject = hit.collider.gameObject;                // 현재 감지된 오브젝트를 저장
                    curInteractable = hit.collider.GetComponent<IInteractable>();   // IInteractable 인터페이스를 가진 오브젝트인지 체크
                    
                    SetPromptText();    // 위 정보를 모두 담아 놨다면 프롬포트에 출력해주기. 감지된 오브젝트에 대한 프롬프트(UI 텍스트) 설정
                }
            }
            else // 아무 오브젝트도 감지되지 않았을 경우 (빈 공간을 바라볼 때)
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false); // 프롬프트 텍스트 숨기기
            }
        }
    }
    
    // 현재 감지된 오브젝트의 상호작용 메시지를 UI에 표시하는 함수
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);  // 프롬프트 텍스트 UI 활성화
        promptText.text = curInteractable.GetInteractPrompt();  // GetInteractPrompt 기능 가져오기. 감지된 오브젝트의 상호작용 메시지를 가져와 표시
    }
    
    // 플레이어가 상호작용 키(E)를 눌렀을 때 실행되는 함수
    // Interaction이라는 스크립트를 Player에게 컴포넌트로 붙여주게 되면 E 키를 눌렀을 때 효과가 날 수 있도록 이벤트 달아주기
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // 키를 눌렀고, 상호작용 가능한 오브젝트가 있을 경우 실행
        if (context.phase == InputActionPhase.Started &&
            curInteractable != null) // 눌렸을 때 && 아이템을 바라보고 있을 때(캐싱하고 있는 정보가 있을 때)
        {
            curInteractable.OnInteract();   // 상호작용 수행 (curInteractable에 있는 OnInteract() 실행)
            curInteractGameObject = null;   // 현재 감지된 오브젝트 초기화
            curInteractable = null;
            promptText.gameObject.SetActive(false); // 프롬프트 텍스트 숨기기
        }
    }
}
