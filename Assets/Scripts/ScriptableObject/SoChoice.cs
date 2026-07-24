using UnityEngine;

public enum StatType
{
    HP,
    Will,
    Fear,
    Stress,
    Attack,
    Defense
}

[CreateAssetMenu(fileName = "New Choice", menuName = "Text Adventure/Choice")]
public class SoChoice : ScriptableObject
{
    [TextArea(2, 4)]
    public string optionText;

    [TextArea(3, 6)]
    public string positiveConsequence;   

    [TextArea(3, 6)]
    public string negativeConsequence;

    [Header("Efeito")]
    public StatType affectedStat;   
    public float amount;            

    [Header("Próxima Fase")]
    public SoPhase nextPhase;
}
