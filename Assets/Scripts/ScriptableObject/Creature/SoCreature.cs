using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Text Adventure/Creature")]
public class SoCreature : ScriptableObject
{
    public string creatureName;

    [Header("Stats")]
    public float maxHP;
    public float attack;
    public float defense;
    public float creatureWill;
}
