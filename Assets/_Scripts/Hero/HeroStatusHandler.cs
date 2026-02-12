using System;
using UnityEngine;

/*
    DB에서 Key / Value로 이루어진 Dictional를 통하여
    해당 클라이언트의 유닛인
    Key : Hero ID값 / Value : Level값 을 통하여 데이터 핸들링 하는 작업 할 공간.
 */

public class HeroStatusHandler : MonoBehaviour
{
    [SerializeField] private HeroData _baseHeroData; //원본DB <- CSV 파서 들여오면 그걸로 수정. (tableName = table_herostat)

    private int _level;
    private int _exp;
    private bool _isUnlock;

    // 영웅 최신 데이터정보(레벨값 반영)
    private HeroStatus _currentHeroData;

    // 프로퍼티
    public HeroStatus CurrentHeroData => _currentHeroData;

    // 이벤트
    public event Action<string, int, int, bool> OnHeroDataChanged;

    public int Level
    {
        get => _level;
        set { _level = value; NotifyDataChanged(); }
    }

    public int Exp
    {
        get => _exp;
        set { _exp = value; NotifyDataChanged(); }
    }

    private void NotifyDataChanged()
    {
        OnHeroDataChanged?.Invoke(_baseHeroData.Hero_ID, _level, _exp, _isUnlock);
    }

    public void Initialize(int level, int exp, bool isUnlock)
    {
        _level = level;
        _exp = exp;
        _isUnlock = isUnlock;
        RefreshData();
    }

    // 레벨에 따른 Status변화 
    private void RefreshData()
    {
        if (_currentHeroData != null)
        {
            if (_baseHeroData == null || _baseHeroData.HeroStatus == null) return;

            _currentHeroData = new HeroStatus(_baseHeroData.HeroStatus);

            ApplyLevelStatus(_currentHeroData, _level);
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
