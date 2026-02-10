using UnityEngine;
using UnityEngine.UI;
using static Fusion.Sockets.NetBitBuffer;

public class HpBarController : MonoBehaviour
{
    [SerializeField] private Image hpFill;
    [SerializeField] private Vector3 _offset = new Vector3(0, 2.5f, 0); //머리위 높이
    private HealthBase _target;

    public void Setup(HealthBase target)
    {
        _target = target;
        UpdateVisual(target.CurrentHp, target.MaxHp);
        _target.OnHpChanged += UpdateVisual;
    }

    public void UpdateVisual(float currentHp, float maxHp)
    {
        hpFill.fillAmount = currentHp / maxHp;
    }

    private void LateUpdate()
    {
        if(_target == null) return;

        transform.position = Camera.main.WorldToScreenPoint(_target.transform.position + _offset);
    }

    private void OnDisable()
    {
        if (_target != null) _target.OnHpChanged -= UpdateVisual;
        _target = null;
    }
}
