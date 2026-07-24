using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float startHP;
    public float currentHP;
    public float maxHP;

    [Header("Will")]
    public float startWill;
    public float currentWill;

    [Header("Fear")]
    public float startFear;
    public float currentFear;

    [Header("Mana")]
    public float startMana;
    public float currentMana;
    public float maxMana;

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
        currentHP = maxHP;
        currentMana = maxMana;
        currentWill = startWill; 
        currentFear = startFear;
        currentAttack = baseAttack; 
        currentDefense = baseDefense;
        disbelief = Startdisbelief;
    }

}
