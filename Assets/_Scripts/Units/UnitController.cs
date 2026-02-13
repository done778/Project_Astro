using UnityEngine;


public abstract class UnitController : MonoBehaviour
{
    //protected UnitStat stat; //일단 임시스텟을 사용할 예정
    //주입 받을 데이터들(공격범위,공격쿨타임 등등)

    //public virtual bool IsDead => stat != null && stat.CurrentHealth <= 0f;

    [Header("Prototype Stat")]
    [SerializeField] protected float _maxHealth = 100f;
    [SerializeField] protected float _currentHealth = 100f;
    [SerializeField] protected float _attackDamage = 10f;
    [SerializeField] protected float _attackRange = 1.5f;
    [SerializeField] protected float _attackCooldown = 1f;

    [Header("Pool")]
    [SerializeField] protected string _poolTag;

    protected float _nextAttackTime;

    public float AttackRange => _attackRange;
    public virtual bool IsDead => _currentHealth <= 0f;
    

    protected virtual void Awake()
    {
        //stat = GetComponent<UnitStat>();
        _currentHealth = _maxHealth;
    }
    protected virtual void OnEnable()
    {
        _currentHealth = _maxHealth;
        _nextAttackTime = 0f;
    }

    protected virtual bool CanAttack()
    {
        return Time.time >= _nextAttackTime;
    }

    protected virtual void AttackCooldown()
    {
        _nextAttackTime = Time.time + _attackCooldown;
    }

    public abstract void Attack(Transform target);

    public virtual bool InAttackRange(Transform target)
    {
        if (target == null)
        {
            return false;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        return distance <= _attackRange;
    }
    public virtual void TakeDamage(float damage)
    {
        if (IsDead)
        {
            return;
        }

        _currentHealth -= damage;

        if (_currentHealth <= 0f)
        {
            _currentHealth = 0f;
            Die();
        }
    }

    protected virtual void Die()
    {
        // 풀 반환
        PoolManager.Instance.ReturnToPool(_poolTag, gameObject);
    }
}
