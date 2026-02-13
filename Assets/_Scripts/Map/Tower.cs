using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private float _hp;
    public event Action OnTowerDestroyed;
    private bool _isDestroyed = false;

    //26-02-13 주현중 수정 (임의로 범위,공격력 등등을 설정해서 진행)
    [SerializeField] private float _damage;
    [SerializeField] private float _detectRange;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _scanInterval = 0.5f;
    [SerializeField] private float _attackInterval = 1f;

    private UnitController _currentTarget;
    private float _nextScanTime;
    private float _nextAttackTime;

    private void Update()
    {
        if (_isDestroyed)
        {
            return;
        }

        if (Time.time < _nextAttackTime)
        {
            return;
        }

        UnitController target = FindTarget();
        if (target == null)
        {
            return;
        }

        target.TakeDamage(_damage);

        _nextAttackTime = Time.time + _attackInterval;
    }

    private UnitController FindTarget()
    {
        if (Time.time < _nextScanTime)
        {
            return _currentTarget != null && !_currentTarget.IsDead ? _currentTarget : null;
        }

        _nextScanTime = Time.time + _scanInterval;

        Collider[] hits = Physics.OverlapSphere(transform.position, _detectRange, _targetLayer);

        if (hits.Length == 0)
        {
            _currentTarget = null;
            return null;
        }

        float minDistance = float.MaxValue;
        UnitController closest = null;

        foreach (var hit in hits)
        {
            UnitController unit = hit.GetComponent<UnitController>();

            if (unit == null || unit.IsDead)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = unit;
            }
        }

        _currentTarget = closest;
        return _currentTarget;
    }

    public void TakeDamage(float amount)
    {
        if (_isDestroyed) return;

        _hp -= amount;
        if (_hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        _isDestroyed = true;
        OnTowerDestroyed?.Invoke();
        gameObject.SetActive(false);
    }
}
