using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    public PlayerStats playerStats;
    public Transform transform_statsArea;
    public Image image_playerPortrait;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image_playerPortrait.sprite = playerStats.portrait;   
    }

    // Update is called once per frame
    void Update()
    {
        transform_statsArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerStats.currentVitality.ToString();
        transform_statsArea.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerStats.currentStress.ToString();
        transform_statsArea.GetChild(2).GetComponent<TextMeshProUGUI>().text = playerStats.currentWill.ToString();
        transform_statsArea.GetChild(3).GetComponent<TextMeshProUGUI>().text = playerStats.currentFear.ToString();
        transform_statsArea.GetChild(4).GetComponent<TextMeshProUGUI>().text = playerStats.currentAttack.ToString();
        transform_statsArea.GetChild(5).GetComponent<TextMeshProUGUI>().text = playerStats.currentDefense.ToString();
    }
}
