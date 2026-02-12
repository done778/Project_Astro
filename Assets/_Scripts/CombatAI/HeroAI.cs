using UnityEngine;
using Fusion;
using System.Collections.Generic;


/// <summary>
/// 영웅 자동 전투 AI
/// </summary>
public class HeroAI : BaseAutoBattleAI
{
    [Header("목표")]
    [SerializeField] private Transform _enemyBase;//최종 목표

    //영웅 전용 데이터 (Stat / Role / Skill 등) 추가 예정

    protected override void Awake()
    {
        base.Awake();

        if (_enemyBase != null)
        {
            finalGoal = _enemyBase.position;
        }
    }

    //public override void Spawned()//네트워크 연결전이라 주석처리
    //{
    //    base.Spawned();

    //    //if (!Object.HasStateAuthority)
    //    //{
    //    //    return;
    //    //}

    //    //전진 목표 설정
    //    if (_enemyBase != null)
    //    {
    //        advancePoint = _enemyBase.position;
    //    }
    //}

    protected override void UpdateState()
    {
        switch (currentState)
        {
            case AutoBattleState.Advance:
                UpdateAdvance();
                break;

            case AutoBattleState.Combat:
                UpdateCombat();
                break;
        }
    }

    private void UpdateAdvance()
    {
        //if (!HasAdvancePoint())//네트워크 연결전이라 주석처리
        //{
        //    return;
        //}

        MoveTo(finalGoal);

        if (FindTarget())
        {
            ChangeState(AutoBattleState.Combat);
        }
    }

    private void UpdateCombat()
    {
        if (!IsTargetValid())
        {
            ChangeState(AutoBattleState.Advance);
            return;
        }

        //영웅 전용 전투 로직 (스킬 / 역활 기반 분기) 추가 예정

        MoveTo(currentTarget.position);
    }

    //전투상태로 들어갈수있는지 판단하는 메서드 추가 예정

}
