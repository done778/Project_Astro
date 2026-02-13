using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private float _hp;
    public event Action OnTowerDestroyed;
    private bool _isDestroyed = false;

    //26-02-13 주현중 수정 (임의로 범위,공격력을 설정해서 진행)
    [SerializeField] private float _damage;
    [SerializeField] private float _detectRange;


    public void TakeDamage(float amount)
    {
        if(_isDestroyed) return;

        _hp -= amount;
        if(_hp <= 0)
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
