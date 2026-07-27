using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour
{
    [Header("References")]
    public CombatPlayer player;
    public Enemy enemy;
    public CombatUI combatUI;

    [Header("Runtime")]
    private bool isPlayerTurn;

    public void StartCombat()
    {
        if (player == null || enemy == null)
        {
            Debug.LogError("CombatManager: faltam referências de Player ou Enemy!");
            return;
        }

        // Liga o painel de combate
        if (combatUI != null)
            combatUI.ShowCombatPanel();

        // Decide quem começa comparando Will
        float playerWill = player.playerStats.currentWill;
        float enemyWill = enemy.creatureData.creatureWill;

        isPlayerTurn = playerWill >= enemyWill;

        Debug.Log(isPlayerTurn ? "Player começa!" : "Inimigo começa!");

        // Inicia o loop de combate
        StartCoroutine(CombatLoop());
    }

    IEnumerator CombatLoop()
    {
        // Enquanto ninguém morreu
        while (player.currentHP > 0 && enemy.currentHP > 0)
        {
            if (isPlayerTurn)
            {
                yield return StartCoroutine(PlayerTurn());
            }
            else
            {
                yield return StartCoroutine(EnemyTurn());
            }

            // Alterna o turno
            isPlayerTurn = !isPlayerTurn;
        }

        // Combate acabou
        EndCombat();
    }

    IEnumerator PlayerTurn()
    {
        Debug.Log("Turno do Player");
        // Aqui depois entram as ações do player
        yield return new WaitForSeconds(1f); // placeholder
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Turno do Inimigo");
        // Aqui depois entra a IA do inimigo
        yield return new WaitForSeconds(1f); // placeholder
    }

    void EndCombat()
    {
        // Desliga o painel
        if (combatUI != null)
            combatUI.HideCombatPanel();

        if (player.currentHP <= 0)
        {
            Debug.Log("Player perdeu o combate!");
            // TODO: consequência de derrota
        }
        else
        {
            Debug.Log("Player venceu o combate!");
            // TODO: consequência de vitória + progressão
        }
    }
}
