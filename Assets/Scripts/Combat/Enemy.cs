using System.Collections.Generic;
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

       
    }
    public void SetupFromCreature(SoCreature data)
    {
        if (data == null)
        {
            Debug.LogWarning("Enemy: SoCreature está nulo!");
            return;
        }

        creatureData = data;

        // Transforma os dados do ScriptableObject em variáveis de runtime
        currentHP = data.maxHP;
        baseAttack = data.attack;
        baseDefense = data.defense;

        Debug.Log($"Enemy configurado: {data.creatureName} | HP: {currentHP} | ATK: {baseAttack} | DEF: {baseDefense}");
    }
    /// <summary>
    /// Escolhe um ataque aleatório da lista e calcula o dano.
    /// Retorna o valor do dano causado.
    /// </summary>
    public int PerformAttack(CombatPlayer target)
    {
        if (creatureData == null || creatureData.attacks == null || creatureData.attacks.Count == 0)
        {
            Debug.LogWarning("Enemy: Sem ataques disponíveis!");
            return 0;
        }

        if (target == null)
        {
            Debug.LogWarning("Enemy: Alvo nulo!");
            return 0;
        }

        // Escolhe um ataque aleatório
        SoAttack chosenAttack = creatureData.attacks[Random.Range(0, creatureData.attacks.Count)];

        // Cálculo básico de dano (por enquanto)
        float rawDamage = chosenAttack.baseDamage + baseAttack;
        float finalDamage = rawDamage - target.playerStats.currentDefense;

        // Impede dano negativo
        if (finalDamage < 0) finalDamage = 0;

        int damageToDeal = Mathf.RoundToInt(finalDamage);

        Debug.Log($"{creatureData.creatureName} usou {chosenAttack.attackName} e causou {damageToDeal} de dano!");

        return damageToDeal;
    }
}
