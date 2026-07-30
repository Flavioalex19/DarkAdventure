using UnityEngine;
using TMPro;

public class BtnAttack : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textAtkName;

    [Header("Data")]
    public SoAttack soAtk;

    [Header("Combat Chances")]
    public float accuracy = 100f;           
    public float criticalChance = 0f;

    [Header("References")]
    public Enemy enemy;
    public CombatManager combatManager;

    [Header("Runtime")]
    public int damage;
    public PlayerStats p_stats;
    public AttackType attackType;
    public AttackEffect attackEffect;

    /// <summary>
    /// Copia os valores do ScriptableObject para as variáveis do botão
    /// </summary>
    public void SetupFromScriptableObject()
    {
        if (soAtk == null)
        {
            Debug.LogWarning("BtnAttack: SoAttack não atribuído!");
            return;
        }

        // Nome
        if (textAtkName != null)
            textAtkName.text = soAtk.attackName;

        // Valores
        damage = Mathf.RoundToInt(soAtk.baseDamage);
        attackType = soAtk.attackType;
        attackEffect = soAtk.attackEffect;
    }
    /// <summary>
    /// Função de cálculo de dano
    /// </summary>
    public float CalculateDamage()
    {
        if (p_stats == null || soAtk == null) return 0f;

        // 1. Chance de acertar (Accuracy)
        float hitRoll = Random.Range(0f, 100f);
        if (hitRoll > accuracy)
        {
            Debug.Log("Errou o ataque!");
            return 0f; // Errou
        }

        // 2. Dano base inicial
        float damage = soAtk.baseDamage + p_stats.currentAttack;

        // 3. Modificadores por tipo de ataque
        switch (attackType)
        {
            case AttackType.Normal:
                // 100% de potência, sem alteração
                break;

            case AttackType.Stress:
                // Stress reduz a porcentagem de ataque
                // Quanto mais Stress o player tiver, mais reduz
                float stressPenalty = p_stats.currentStress / 100f; // 0 a 1
                damage *= (1f - stressPenalty);
                break;

            case AttackType.Fear:
                // Fear reduz a potência do ataque
                float fearPenalty = p_stats.currentFear / 100f;
                damage *= (1f - fearPenalty * 0.7f); // Fear reduz até 70% da potência
                break;

            case AttackType.Willpower:
                // Willpower aumenta a chance de crítico
                criticalChance = p_stats.currentWill; // quanto maior o Will, maior a chance
                break;
        }

        // 4. Chance de crítico (principalmente no Willpower, mas pode valer pra todos)
        float critRoll = Random.Range(0f, 100f);
        if (critRoll <= criticalChance)
        {
            damage *= 1.5f; // Crítico = 150% do dano
            Debug.Log("CRITICAL HIT!");
        }

        // Evita dano negativo
        if (damage < 0) damage = 0;

        return damage;
    }

    /// <summary>
    /// Aplica o dano no inimigo
    /// </summary>
    public void DealDamageToEnemy()
    {
        if (enemy == null)
        {
            Debug.LogWarning("BtnAttack: Enemy não atribuído!");
            return;
        }

        float finalDamage = CalculateDamage();

        enemy.currentHP -= finalDamage;

        if (enemy.currentHP < 0)
            enemy.currentHP = 0;

        Debug.Log($"Dano causado: {finalDamage} | HP restante do inimigo: {enemy.currentHP}");
    }
    public void OnClickAttack()
    {
        if (combatManager != null)
        {
            combatManager.OnPlayerAttack(this);
        }
    }
}
