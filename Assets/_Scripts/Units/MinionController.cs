using UnityEngine;

public enum AttackType
{
    Melee, Range
}
public class MinionController : UnitController
{
    [Header("공격 타입")]
    [SerializeField] private AttackType _attackType;

    [Header("원거리")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;

    public override void Attack(Transform target)
    {
        if (IsDead || target == null)
        {
            return;
        }

        if (!CanAttack())
        {
            return;
        }
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > _attackRange)
        {
            return;
        }

        switch (_attackType)
        {
            case AttackType.Melee:
                AttackMelee(target);
                break;

            case AttackType.Range:
                AttackRanged(target);
                break;
        }

        AttackCooldown();
    }

    private void AttackMelee(Transform target)
    {
        Tower tower = target.GetComponent<Tower>();
        if (tower != null)
        {
            tower.TakeDamage(_attackDamage);
            return;
        }

        UnitController unit = target.GetComponent<UnitController>();
        if (unit != null && !unit.IsDead)
        {
            unit.TakeDamage(_attackDamage);
        }
    }

    private void AttackRanged(Transform target)
    {
        if (_projectilePrefab == null || _firePoint == null)
        {
            return;
        }

        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);

        projectile.GetComponent<Projectile>().Fire(target, _attackDamage);
    }
}
