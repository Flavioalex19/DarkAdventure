using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    [Header("References")]
    public SoCard card;
    public PlayerStats playerStats;
    public TextMeshProUGUI descriptionText;
    public AdventureManager manager;
    void Awake()
    {
        // Configura o OnClick automaticamente
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnCardSelected);
        }
    }
    private void Start()
    {
        manager = GameObject.Find("AdventureManager").GetComponent<AdventureManager>();
    }

    public void OnCardSelected()
    {
        Debug.Log("Carta clicada!");

        if (card == null)
        {
            Debug.LogWarning("Card está nulo!");
            return;
        }

        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats está nulo!");
            return;
        }

        // Mostra a descrição
        if (descriptionText != null)
            descriptionText.text = card.description;

        // Aplica os efeitos
        ApplyStatChange(card.positiveStat, card.positiveAmount);
        ApplyStatChange(card.negativeStat, -card.negativeAmount);

        // Avisa o Manager
        if (manager != null)
        {
            Debug.Log("Avisando o Manager...");
            manager.OnCardSelected();
        }
        else
        {
            Debug.LogWarning("Manager está nulo no CardButton!");
        }
    }

    void ApplyStatChange(StatType stat, float amount)
    {
        switch (stat)
        {
            case StatType.HP: playerStats.currentHP += amount; break;
            case StatType.Will: playerStats.currentWill += amount; break;
            case StatType.Fear: playerStats.currentFear += amount; break;
            case StatType.Stress: playerStats.currentStress += amount; break;
            case StatType.Attack: playerStats.currentAttack += amount; break;
            case StatType.Defense: playerStats.currentDefense += amount; break;
        }
    }

    // Método auxiliar pra atualizar o texto do botão (usado pelo PrepareCards)
    public void UpdateButtonText()
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null && card != null)
        {
            buttonText.text = card.cardName;
        }
    }
}
