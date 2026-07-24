using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Text Adventure/Card")]
public class SoCard : ScriptableObject
{
    public string cardName;

    [TextArea(3, 6)]
    public string description;

    [Header("Efeito Positivo")]
    public StatType positiveStat;     
    public float positiveAmount;      

    [Header("Efeito Negativo")]
    public StatType negativeStat;     
    public float negativeAmount;      
}
