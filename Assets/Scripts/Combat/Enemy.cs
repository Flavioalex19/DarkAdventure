using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Data")]
    public SoCreature creatureData;

    [Header("Runtime Stats")]
    public float currentHP;
    public float baseAttack;
    public float baseDefense;

    void Start()
    {
        if (creatureData == null)
        {
            Debug.LogWarning("Enemy sem CreatureData atribuída!");
            return;
        }

        // Atribuições iniciais
        currentHP = creatureData.maxHP;
        baseAttack = creatureData.attack;
        baseDefense = creatureData.defense;
    }
}
