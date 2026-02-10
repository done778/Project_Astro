using UnityEngine;
using UnityEngine.AI;
using Fusion;

//일단 임의로 상태 분리해둠
public enum AutoBattleState { Advance, Search, Return }

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

    private float _nextScanTime;

    protected virtual void Awake()
    {
        //기본 상태는 전진
        currentState = AutoBattleState.Advance;
    }

    public override void Spawned()
    {
        //첫 스캔 시점을 동일하지 않게(순간적인 프레임드랍 방지)
        _nextScanTime = Time.time + Random.Range(0f, scanInterval);
    }

    public override void FixedUpdateNetwork()
    {
        //AI 판단은 State Authority를 가진 클라이언트만 수행(PUN의 마스터클라이언트와 유사한듯?)
        if (!Object.HasStateAuthority)
        {
            return;
        }

        UpdateState();
    }

    protected abstract void UpdateState();//상태 업데이트

    protected void FindTarget()//가까운 적 거리 기준 찾기
    {
        if (Time.time < _nextScanTime)
        {
            return;
        }

        _nextScanTime = Time.time + scanInterval;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, targetLayer);

        if (hits.Length == 0)
        {
            currentTarget = null;
            return;
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
    }

    protected virtual void MoveTo(Vector3 destination)
    {
        //현재는 네비메쉬 기반, 추후 변경될수도 있음
        if (agent == null || !agent.enabled)
        {
            return;
        }

        agent.SetDestination(destination);
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()//탐지 범위 시각화
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
#endif
}
