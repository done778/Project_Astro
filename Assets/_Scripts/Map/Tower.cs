using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private float _hp; 
    public event Action OnTowerDestroyed;
    private bool _isDestroyed = false;
    

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
