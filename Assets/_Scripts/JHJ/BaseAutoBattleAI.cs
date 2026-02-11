using UnityEngine;
using UnityEngine.AI;
using Fusion;

//일단 임의로 상태 분리해둠
public enum AutoBattleState { Advance, Combat }

/// <summary>
/// 자동 전투 공통 AI로직 담당
/// 상태 전환,탐지,이동, 유효성 검사
/// 상태별 행동은 파생 클래스에서 구현
/// </summary>
public abstract class BaseAutoBattleAI : NetworkBehaviour
{
    [Header("네비")]
    [SerializeField] protected NavMeshAgent agent;

    [Header("탐지")]
    [SerializeField] protected float detectRadius = 10f;//탐지 범위
    [SerializeField] protected LayerMask targetLayer;//탐지 대상
    [SerializeField] protected float scanInterval = 0.5f;//스캔간격0.5초

    protected AutoBattleState currentState;
    protected Transform currentTarget;
    protected Vector3 advancePoint;//목표 지점

    private float _nextScanTime;

    protected virtual void Awake()
    {
        //기본 상태는 전진
        currentState = AutoBattleState.Advance;
        _nextScanTime = Time.time + Random.Range(0f, scanInterval);
    }

    //public override void Spawned()//Spawn 완료됐을 때 호출되는 Fusion 콜백
    //{
    //    //AI 판단은 State Authority를 가진 클라이언트만 수행(PUN의 마스터클라이언트와 유사)
    //    if (!Object.HasStateAuthority)
    //    {
    //        return;
    //    }

    //    //첫 스캔 시점을 동일하지 않게(순간적인 프레임드랍 방지)
    //    float now = (float)Runner.SimulationTime;
    //    _nextScanTime = now + Random.Range(0f, scanInterval);
    //}

    //public override void FixedUpdateNetwork()
    //{
    //    //if (!Object.HasStateAuthority)
    //    //{
    //    //    return;
    //    //}

    //    UpdateState();
    //}

    private void Update()//네트워크 적용전 업데이트로 테스트
    {
        if (Runner == null)
        {
            UpdateState();
        }
    }

    protected abstract void UpdateState();//상태 업데이트(파생클래스에서 구현)

    protected void ChangeState(AutoBattleState nextState)//상태 전환 메서드
    {
        if (currentState == nextState)
        {
            return;
        }

        StopMove();
        currentState = nextState;
    }

    protected bool FindTarget()//가까운 적 거리 기준 찾기
    {
        //float now = (float)Runner.SimulationTime;
        //if (now < _nextScanTime)
        //{
        //    return currentTarget != null;
        //}

        //_nextScanTime = now + scanInterval;

        if (Time.time < _nextScanTime)
        {
            return currentTarget != null;
        }

        _nextScanTime = Time.time + scanInterval;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, targetLayer);

        if (hits.Length == 0)
        {
            currentTarget = null;
            return false;
        }

        float minDistance = float.MaxValue;
        Transform closest = null;

        foreach (var hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = hit.transform;
            }
        }

        currentTarget = closest;
        return currentTarget != null;
    }

    protected bool IsTargetValid()//타겟 확인용 메서드
    {
        if (currentTarget == null)
        {
            return false;
        }

        return currentTarget.gameObject.activeInHierarchy;
    }

    protected bool HasAdvancePoint()//목표 확인용 메서드
    {
        return advancePoint != Vector3.zero;
    }

    protected virtual void MoveTo(Vector3 destination)
    {
        //현재는 네비메쉬 기반, 추후 변경될수도 있음
        if (agent == null || !agent.enabled)
        {
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    protected virtual void StopMove()
    {
        if (agent == null || !agent.enabled)
        {
            return;
        }

        agent.isStopped = true;
        agent.ResetPath();
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()//탐지 범위 시각화
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
#endif
}
