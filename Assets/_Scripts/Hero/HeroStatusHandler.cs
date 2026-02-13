using UnityEngine;

/*
    DB에서 Key / Value로 이루어진 Dictionary를 통하여
    해당 클라이언트의 유닛인
    Key : Hero ID값 / Value : Level값 을 통하여 데이터 핸들링 하는 작업 할 공간.
    아웃 게임 요소로 레벨에 따른 능력치 변화 값만 계산
 */

public class HeroStatusHandler : MonoBehaviour
{
    [SerializeField] private HeroData _baseHeroData; //원본DB

    //영웅 최신 데이터정보(레벨값 반영)
    private HeroStatus _currentHeroData;
    private int _currentLevel;

    //프로퍼티
    public HeroStatus CurrentHeroData => _currentHeroData;
    public int CurrentLevel => _currentLevel;

    private void Start()
    {
        Initialize(1);
    }

    public void Initialize(int level)
    {
        _currentLevel = level;
        RefreshData();
    }

    //레벨에 따른 Status변화 
    private void RefreshData()
    {
        if (_currentHeroData != null)
        {
            if (_baseHeroData == null || _baseHeroData.HeroStatus == null) return;

            _currentHeroData = new HeroStatus(_baseHeroData.HeroStatus);

            ApplyLevelStatus(_currentHeroData, _currentLevel);
        }
    }

    private void ApplyLevelStatus(HeroStatus status, int level)
    {
        //레벨업 시 상승되는 스테이터스 상승치 로직 구현하기
        //하기에 예시 살짞
        float growthRate = 0.1f * level;
        status.Hp += status.Hp * growthRate;
    }
}
