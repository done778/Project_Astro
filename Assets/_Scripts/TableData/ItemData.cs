using System;

[Serializable]
public class ItemData : ITableData
{
    public int ItemID;
    public string Name;
    public int Type;
    public int EffectGroupID;
    public string IconImage;
    public bool IsStackable;
    public string Description;

    public string PrimaryID => ItemID.ToString();
}
