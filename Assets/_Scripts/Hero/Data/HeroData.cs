using UnityEngine;

public enum HeroType { Robot, SpaceCraft }
public enum HeroRole { Tank, Melee, Ranged, Summoner, Healer }
public enum ArmorType { Light_Armor, Medium_Armor, Heavy_Armor }

[CreateAssetMenu(fileName = "HeroData", menuName = "ScriptableObject/HeroData")]
public class HeroData : ScriptableObject
{
    [Header("Hero Base Data")]
    public string Hero_ID;
    public string Hero_Name;
    public bool IsDefault;
    public int Hero_Gold_Requirement;

    public HeroType Hero_Type; 
    public HeroRole Hero_Role;
    public ArmorType Armor_Type;

    [TextArea(3,5)]
    public string Hero_Description;

    [Header("Hero Status")]
    public HeroStatus HeroStatus;

    [Header("Hero Visual")]
    public Sprite Hero_Icon; 
    public Sprite Hero_Image;
    public string Hero_Modeling; // 경로
    public string Hero_PreviewVideo; // 경로
}
