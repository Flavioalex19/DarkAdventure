using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{

    [Header("References")]
    public GameObject combatPanel;
    public Transform combatButtons;          
    public CombatManager combatManager;

    [Header("Health Bars")]
    public Image fillAmountHPPlayer;
    public Image fillAmountHPEnemy;

    [Header("Feedback - Player")]
    public List<string> playerAttackMessages;       
    public List<string> playerHitMessages;          
    public List<string> playerMissMessages;         

    [Header("Feedback - Enemy")]
    public List<string> enemyAttackMessages;        
    public List<string> enemyHitMessages;           
    public List<string> enemyMissMessages;

    [Header("Feedback Text")]
    public TextMeshProUGUI textAttackFeedback;

    [Header("Feedback Settings")]
    public float delayTime = 3f;


    void Start()
    {
        // Só para teste: monta os botões no Start
        SetupAttackButtons();
    }

    public void ShowCombatPanel()
    {
        if (combatPanel != null)
            combatPanel.SetActive(true);
    }

    public void HideCombatPanel()
    {
        if (combatPanel != null)
            combatPanel.SetActive(false);
    }

    /// <summary>
    /// Monta os botões de ataque aleatoriamente (sem repetir)
    /// </summary>
    public void SetupAttackButtons()
    {
        if (combatManager == null || combatButtons == null) return;

        // Junta todas as listas em uma só temporária
        List<SoAttack> allAttacks = new List<SoAttack>();

        if (combatManager.normalAttacks != null) allAttacks.AddRange(combatManager.normalAttacks);
        if (combatManager.stressAttacks != null) allAttacks.AddRange(combatManager.stressAttacks);
        if (combatManager.fearAttacks != null) allAttacks.AddRange(combatManager.fearAttacks);
        if (combatManager.willpowerAttacks != null) allAttacks.AddRange(combatManager.willpowerAttacks);

        // Embaralha e remove duplicatas (pelo nome, por segurança)
        allAttacks = allAttacks
            .Where(a => a != null)
            .GroupBy(a => a.attackName)
            .Select(g => g.First())
            .OrderBy(x => Random.value)
            .ToList();

        // Pega quantos botões filhos existem
        int buttonCount = combatButtons.childCount;

        for (int i = 0; i < buttonCount; i++)
        {
            Transform buttonTransform = combatButtons.GetChild(i);
            BtnAttack btnAttack = buttonTransform.GetComponent<BtnAttack>();

            if (btnAttack == null) continue;

            if (i < allAttacks.Count)
            {
                // Atribui o ataque
                btnAttack.soAtk = allAttacks[i];

                // Chama a função de setup do botão
                btnAttack.SetupFromScriptableObject();
            }
            else
            {
                // Se não tiver ataque suficiente, desativa o botão
                buttonTransform.gameObject.SetActive(false);
            }
        }
    }
    public void UpdateHealthBars(float playerCurrentHP, float playerMaxHP, float enemyCurrentHP, float enemyMaxHP)
    {
        // Player
        if (fillAmountHPPlayer != null && playerMaxHP > 0)
        {
            fillAmountHPPlayer.fillAmount = playerCurrentHP / playerMaxHP;
        }

        // Enemy
        if (fillAmountHPEnemy != null && enemyMaxHP > 0)
        {
            fillAmountHPEnemy.fillAmount = enemyCurrentHP / enemyMaxHP;
        }
    }
    /// <summary>
    /// Mostra uma mensagem simples (sem typewriter)
    /// </summary>
    public void ShowFeedback(string message)
    {
        if (textAttackFeedback != null)
            textAttackFeedback.text = message;
    }

    /// <summary>
    /// Sorteia uma mensagem de uma lista
    /// </summary>
    public string GetRandomMessage(List<string> list)
    {
        if (list == null || list.Count == 0) return "";
        return list[Random.Range(0, list.Count)];
    }
    public void EnableAttackButtons(bool enable)
    {
        if (combatButtons == null) return;

        for (int i = 0; i < combatButtons.childCount; i++)
        {
            combatButtons.GetChild(i).gameObject.SetActive(enable);
        }
    }
}
