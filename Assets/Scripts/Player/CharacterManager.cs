using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // 싱글톤 패턴을 사용해 게임 내에서 하나의 CharacterManager 인스턴스만 존재하도록 설정
    private static CharacterManager _instance;

    // CharacterManager의 유일한 인스턴스를 가져오는 프로퍼티
    public static CharacterManager Instance
    {
        get
        {   
            // 인스턴스가 없으면
            if (_instance == null)
            {
                // 새 GameObject를 생성하고 CharacterManager를 추가
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    public Player _player;  // 현재 플레이어 객체
    public Player Player    // 플레이어 객체에 접근할 수 있는 프로퍼티
    {
        get { return _player; }
        set { _player = value; }
    }
    
    private void Awake()
    {
        // 현재 인스턴스가 존재하지 않으면
        if (_instance == null)
        {
            _instance = this;               // this를 인스턴스로 설정하고
            DontDestroyOnLoad(gameObject);  // 씬 전환 시 파괴되지 않도록 함
        }
        else
        {
            // 이미 인스턴스가 존재한다면
            if (_instance != this)
            {
                Destroy(gameObject);    // 중복 생성을 방지하기 위해 현재 객체를 파괴함
            }
        }
    }
}
