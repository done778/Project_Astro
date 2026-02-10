using UnityEngine;
using System.Collections.Generic;

public class HpBarManager : Singleton<HpBarManager>
{
    [SerializeField] private string hpBarTag = "HP_Bar";
    private Dictionary<HealthBase,HpBarController> _activeBars = new Dictionary<HealthBase,HpBarController>();

    public void ShowHpBar(HealthBase target)
    {
        if (_activeBars.ContainsKey(target)) return;

        GameObject hpBarObj = PoolManager.Instance.SpawnFromPool(hpBarTag,Vector3.zero,Quaternion.identity);
        if (hpBarObj.TryGetComponent(out HpBarController controller))
        {
            controller.Setup(target);
            _activeBars.Add(target, controller);
        }
    }

    public void HideHpBar(HealthBase target)
    {
        if (_activeBars.TryGetValue(target, out HpBarController controller))
        {
            PoolManager.Instance.ReturnToPool(hpBarTag, controller.gameObject);
            _activeBars.Remove(target);
        }
    }
}
