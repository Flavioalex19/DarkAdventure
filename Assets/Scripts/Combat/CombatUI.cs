using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [Header("References")]
    public GameObject combatPanel;

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
}
