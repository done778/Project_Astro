using UnityEngine;

/*
    DB에서 Key / Value로 이루어진 Dictional를 통하여
    해당 클라이언트의 유닛인
    Key : Hero ID값 / Value : Level값 을 통하여 데이터 핸들링 하는 작업 할 공간.
 */

public class HeroStatusHandler : MonoBehaviour
{
    [SerializeField] private HeroData _heroDatas;

    //영웅 최신 데이터정보(레벨값 반영)
    private HeroStatus _refreshHeroData;

    //프로퍼티
    public HeroStatus RefreshHeroData => _refreshHeroData;

    private void Start()
    {
        _refreshHeroData = _heroDatas.HeroStatus;
    }

    //레벨에 따른 Status변화 
    private void RefreshData()
    {
        if (_refreshHeroData != null)
        {
            

        }
    }
}
