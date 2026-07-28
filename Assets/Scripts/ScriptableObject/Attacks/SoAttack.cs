using UnityEngine;

public enum AttackType
{
    Normal,
    Stress,
    Fear,
    Willpower
}
public enum AttackEffect
{
    Normal,
    Stun,
    Poison,
    Confused
}
[CreateAssetMenu(fileName = "New Attack", menuName = "Text Adventure/Attack")]
public class SoAttack : ScriptableObject
{
    public string attackName;

    public float baseDamage;

    public AttackType attackType;

    public AttackEffect attackEffect;
}
