using UnityEngine;
using Fusion;

/// <summary>
/// 미니언 자동 전투 AI
/// </summary>
public class MinionAI : BaseAutoBattleAI
{
    [Header("Advance")]
    [SerializeField] private Transform _enemyBase; //전진 목표
    [SerializeField] private Team team;
    //미니언 전용 데이터 추가 예정

    protected override void Awake()
    {
        base.Awake();
        if (_enemyBase != null)
        {
            advancePoint = _enemyBase.position;
        }
    }

    //public override void Spawned() //네트워크 연결전이라 주석처리
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
                UpdateSearch();
                break;

        }
    }

    private void UpdateAdvance()
    {
        //if (!HasAdvancePoint())//네트워크 연결전이라 주석처리
        //{
        //    return;
        //}

        MoveTo(advancePoint);

        //이동 중 적 탐지
        if (FindTarget())
        {
            ChangeState(AutoBattleState.Combat);
        }
    }

    private void UpdateSearch()
    {
        //타겟이 유효하지 않으면 다시 이동
        if (!IsTargetValid())
        {
            ChangeState(AutoBattleState.Advance);
            return;
        }

        //아직은 공격로직이 없음(공격로직이 생기면 추가할 위치)

        //타겟으로 이동
        MoveTo(currentTarget.position);
    }

    //전투상태로 들어갈수있는지 판단하는 메서드 추가 예정
    public void Setup(Team team, Transform targetBase)
    {
        this.team = team;
        this._enemyBase = targetBase;

        // 진형에 따른 레이어 설정 (예: Blue 미니언은 Red 레이어를 공격)
        if (team == Team.Blue)
        {
            gameObject.layer = LayerMask.NameToLayer("BlueTeam");
            targetLayer = 1 << LayerMask.NameToLayer("RedTeam");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("RedTeam");
            targetLayer = 1 << LayerMask.NameToLayer("BlueTeam");
        }

        if (_enemyBase != null)
        {
            advancePoint = _enemyBase.position;
        }

        // 상태 초기화
        ChangeState(AutoBattleState.Advance);
    }
}
