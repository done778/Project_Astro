using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AutoBattleAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private float _detectRadius = 10f;//탐지범위
    [SerializeField] private LayerMask _targetLayer;//탐지 대상

    [SerializeField] private float _scanInterval = 0.5f;//0.5초

    private Transform _currentTarget;
    private float _nextScanTime = 0f;

    private void Start()
    {
        //첫 스캔 시점을 동일하지 않게(순간적인 프레임드랍 방지)
        _nextScanTime = Time.time + Random.Range(0f, _scanInterval);
    }

    private void Update()
    {   
        if (!IsMaster())//마스터클라이언트만 판단하도록
        {
            return;
        }

        if (_currentTarget == null && Time.time >= _nextScanTime)//타겟이 없거나 유효하지 않으면 다시 탐색
        {
            _nextScanTime = Time.time + _scanInterval;
            FindTarget();
        }
   
        if (_currentTarget != null)//타겟이 있으면 이동
        {
            MoveToTarget();
        }
    }

    private void FindTarget()//가까운 적 거리 기준 찾기
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectRadius, _targetLayer);

        if (hits.Length == 0)
        {
            _currentTarget = null;
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

        _currentTarget = closest;
    }

    private void MoveToTarget()
    {
        if (_agent.enabled == false)
        {
            return;
        }

        _agent.SetDestination(_currentTarget.position);
    }

    private bool IsMaster()//마스터인지 확인용
    {
        return true;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()//탐지 범위 시각화
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectRadius);
    }
#endif
}
