using UnityEngine;

[CreateAssetMenu(fileName = "HeroStatus", menuName = "ScriptableObject/HeroStatus")]
public class HeroStatus : ScriptableObject
{
    public float Base_Hp;
    public float Base_Shield;
    public float Base_Attack_Power;
    public float Base_Healing_Power;
    public float Min_Attack_Speed;
    public float Max_Attack_Speed;
    public float Min_Move_Speed;
    public float Max_Move_Speed;
    public float Min_Spawn_Cooldown;
    public float Max_Spawn_Cooldown;
    
}
