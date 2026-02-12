using UnityEngine;


public abstract class UnitController : MonoBehaviour
{
    public abstract void MoveTo(Vector3 position);
    public abstract void Attack();

    protected virtual void Die() 
    {
        //사망처리
        //풀반환
    }
}
