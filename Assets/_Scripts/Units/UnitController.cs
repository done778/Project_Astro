using UnityEngine;


public abstract class UnitController : MonoBehaviour
{
    protected UnitStat stat;
    //주입 받을 데이터들(공격범위,공격쿨타임 등등)

    public virtual bool IsDead => stat != null && stat.CurrentHealth <= 0f;

    protected virtual void Awake()
    {
        stat = GetComponent<UnitStat>();
    }

    //public abstract bool CanAttack(Transform target);
    public abstract void Attack(Transform target);

    protected virtual void Die()
    {
        // 애니메이션
        // 풀 반환
    }
}
