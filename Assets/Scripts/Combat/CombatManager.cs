using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    [Header("References")]
    public CombatPlayer player;
    public Enemy enemy;
    public CombatUI combatUI;

    [Header("Attack Lists")]
    public List<SoAttack> normalAttacks;
    public List<SoAttack> stressAttacks;
    public List<SoAttack> fearAttacks;
    public List<SoAttack> willpowerAttacks;

    [Header("Runtime")]
    private bool isPlayerTurn;

    public void StartCombat(SoCreature creatureFromPhase)
    {
        // 1. Aloca o inimigo
        AllocateEnemy(creatureFromPhase);

        // 2. Liga o painel de combate
        if (combatUI != null)
        {
            combatUI.ShowCombatPanel();

            // 3. Monta os botões de ataque
            combatUI.SetupAttackButtons();

            // 4. Atualiza as barras de vida no começo do combate
            combatUI.UpdateHealthBars(
                player.currentHP,
                player.maxHP,
                enemy.currentHP,
                enemy.creatureData.maxHP
            );
        }

        // 5. Decide quem começa
        float playerWill = player.playerStats.currentWill;
        float enemyWill = enemy.creatureData.creatureWill;

        isPlayerTurn = playerWill >= enemyWill;

        if (isPlayerTurn)
        {
            combatUI.ShowFeedback("It's player's turn. Choose your attack!");
        }
        else
        {
            combatUI.ShowFeedback("It's enemy's turn");
            StartCoroutine(EnemyTurn());
        }

        Debug.Log(isPlayerTurn ? "Player começa!" : "Inimigo começa!");

        // 6. Inicia o loop de combate
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
        // 1. Desliga os botões do player
        combatUI.EnableAttackButtons(false);

        // 2. Mensagem de que o inimigo vai atacar
        string attackMsg = combatUI.GetRandomMessage(combatUI.enemyAttackMessages);
        combatUI.ShowFeedback(attackMsg);

        yield return new WaitForSeconds(combatUI.delayTime);

        // 3. Inimigo escolhe e executa o ataque
        int damage = enemy.PerformAttack(player);
        bool hit = damage > 0;

        if (hit)
        {
            player.currentHP -= damage;
            if (player.currentHP < 0) player.currentHP = 0;

            string hitMsg = combatUI.GetRandomMessage(combatUI.enemyHitMessages);
            combatUI.ShowFeedback(hitMsg);
        }
        else
        {
            string missMsg = combatUI.GetRandomMessage(combatUI.enemyMissMessages);
            combatUI.ShowFeedback(missMsg);
        }

        // 4. Atualiza barras
        combatUI.UpdateHealthBars(
            player.currentHP,
            player.maxHP,
            enemy.currentHP,
            enemy.creatureData.maxHP
        );

        yield return new WaitForSeconds(combatUI.delayTime);

        // 5. Verifica se o player morreu
        if (player.currentHP <= 0)
        {
            EndCombat();
            yield break;
        }

        // 6. Volta o turno pro player
        isPlayerTurn = true;
        combatUI.ShowFeedback("It's player's turn. Choose your attack!");
        combatUI.EnableAttackButtons(true);
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

    public void AllocateEnemy(SoCreature creatureData)
    {
        if (enemy == null)
        {
            Debug.LogWarning("CombatManager: Enemy não atribuído!");
            return;
        }

        enemy.SetupFromCreature(creatureData);
    }
    public void OnPlayerAttack(BtnAttack btn)
    {
        StartCoroutine(PlayerAttackSequence(btn));
    }

    IEnumerator PlayerAttackSequence(BtnAttack btn)
    {
        // Desliga os botões assim que o player escolhe
        combatUI.EnableAttackButtons(false);
        // 1. Mensagem de que o player está atacando
        string attackMsg = combatUI.GetRandomMessage(combatUI.playerAttackMessages);
        combatUI.ShowFeedback(attackMsg);

        yield return new WaitForSeconds(combatUI.delayTime);

        // 2. Calcula e aplica o dano
        float finalDamage = btn.CalculateDamage();
        bool hit = finalDamage > 0;

        if (hit)
        {
            enemy.currentHP -= finalDamage;
            if (enemy.currentHP < 0) enemy.currentHP = 0;

            string hitMsg = combatUI.GetRandomMessage(combatUI.playerHitMessages);
            combatUI.ShowFeedback(hitMsg);
        }
        else
        {
            string missMsg = combatUI.GetRandomMessage(combatUI.playerMissMessages);
            combatUI.ShowFeedback(missMsg);
        }

        // Atualiza barras
        combatUI.UpdateHealthBars(player.currentHP, player.maxHP, enemy.currentHP, enemy.creatureData.maxHP);

        yield return new WaitForSeconds(combatUI.delayTime);

        // 3. Verifica se o inimigo morreu
        if (enemy.currentHP <= 0)
        {
            EndCombat();
            yield break;
        }

        // 4. Passa para o turno do inimigo
        isPlayerTurn = false;
        combatUI.ShowFeedback("It's enemy's turn");
        yield return new WaitForSeconds(1f);

        StartCoroutine(EnemyTurn());
    }
}
