using System.Collections.Generic;
using UnityEngine;
public enum PhaseType
{
    Default,
    Combat
}
[CreateAssetMenu(fileName = "New Phase", menuName = "Text Adventure/Phase")]
public class SoPhase : ScriptableObject
{
    public string phaseName;

    [TextArea(3, 8)]
    public string description;         

    [Header("Pool de Escolhas")]
    public List<SoChoice> choicePool;

    [Header("Audio")]
    public AudioClip SFXAmbience;

    [Header("Tipo da Fase")]
    public PhaseType phaseType = PhaseType.Default;
    
    [Header("Combat")]
    public SoCreature enemyCreature;
}
