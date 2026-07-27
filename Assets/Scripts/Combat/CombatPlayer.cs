using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
    [Header("Reference")]
    public PlayerStats playerStats;

    [Header("Combat HP")]
    public float maxHP;
    public float currentHP;

    void Start()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("CombatPlayer sem PlayerStats atribuído!");
            return;
        }

    }

    // Exemplos de como pegar os outros stats direto do PlayerStats quando precisar:
    public float GetAttack()
    {
        return playerStats.currentAttack;
    }

    public float GetDefense()
    {
        return playerStats.currentDefense;
    }

    public float GetVitality()
    {
        return playerStats.currentVitality;
    }
}
