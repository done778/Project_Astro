using System;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    [SerializeField] private float maxHp = 100f;
    private float _currentHp;

    public event Action<float, float> OnHpChanged;
    public event Action OnDeath;

    public float CurrentHp => _currentHp;
    public float MaxHp => maxHp;

    private void OnEnable()
    {
        _currentHp = maxHp;
        // 활성화 시 HP바 표시 요청
        if (HpBarManager.Instance != null) HpBarManager.Instance.ShowHpBar(this);
    }

    public void TakeDamage(float amount)
    {
        _currentHp = Mathf.Max(0,_currentHp - amount);
        OnHpChanged?.Invoke(_currentHp, maxHp);

        if(_currentHp <= 0)
        {
            OnDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        // 비활성화 시 HP바 반납 요청
        if (HpBarManager.Instance != null) HpBarManager.Instance.HideHpBar(this);
    }
}
