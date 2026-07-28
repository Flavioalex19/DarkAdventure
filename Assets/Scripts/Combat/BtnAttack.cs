using UnityEngine;
using TMPro;

public class BtnAttack : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textAtkName;

    [Header("Data")]
    public SoAttack soAtk;

    [Header("References")]
    public Enemy enemy;

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
    /// Função de cálculo de dano (vazia por enquanto)
    /// </summary>
    float CalculateDamage()
    {
        // Aqui depois entra a lógica completa de cálculo
        return 0f;
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
}
