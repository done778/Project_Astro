using System;

[Serializable]
public class EffectData : ITableData
{
    public int Index;
    public int EffectGroupID;
    public int EffectType;
    public float EffectValue;
    public int TriggerCondition;
    public float TriggerValue;
    public int TargetType;

    public string PrimaryID => Index.ToString();
}
