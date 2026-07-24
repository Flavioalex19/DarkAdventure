using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Phase", menuName = "Text Adventure/Phase")]
public class SoPhase : ScriptableObject
{
    public string phaseName;

    [TextArea(3, 8)]
    public string description;         

    [Header("Pool de Escolhas")]
    public List<SoChoice> choicePool;
}
