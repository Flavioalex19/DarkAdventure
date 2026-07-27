using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Vitality")]
    public float startVitality;
    public float currentVitality;
    public float maxVitality;

    [Header("Will")]
    public float startWill;
    public float currentWill;

    [Header("Fear")]
    public float startFear;
    public float currentFear;

    [Header("Stress")]
    public float startStress;
    public float currentStress;
    public float maxStress;

    [Header("Attack")]
    public float baseAttack;
    public float currentAttack;

    [Header("Defense")]
    public float baseDefense;
    public float startDefense;
    public float currentDefense;

    [Header("Disbelieve")]
    public int Startdisbelief = 0;
    public int disbelief;

    [Header("Portrait")]
    public Sprite portrait;

    private void Start()
    {
        currentVitality = maxVitality;
        currentStress = maxStress;
        currentWill = startWill; 
        currentFear = startFear;
        currentAttack = baseAttack; 
        currentDefense = baseDefense;
        disbelief = Startdisbelief;
    }

}
