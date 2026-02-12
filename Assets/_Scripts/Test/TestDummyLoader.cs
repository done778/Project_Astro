using UnityEngine;
using Fusion;
public class TestDummyLoader : MonoBehaviour
{
    //테스트용 유닛
    public UnitStat targetUnit;

    private void Start()
    {
        //가짜 아웃게임 데이터 생성 및 주입
        //일단은테이블기준?
        DummyHeroData dummyData = new DummyHeroData();
        dummyData.nickname = "TestPlayer_01";
        dummyData.heroID = 1001;
        dummyData.health = 2500f;  //성장해서 2500
        dummyData.attack = 150f;   //성장150
        dummyData.defense = 30f;   //아래도전부걍
        dummyData.moveSpeed = 5.0f;

        //주입
        if (targetUnit != null) targetUnit.Init(dummyData);
       
        

    }
}
