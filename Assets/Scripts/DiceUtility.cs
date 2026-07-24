using UnityEngine;

public static class DiceUtility
{
    /// <summary>
    /// Rola 2d6 e retorna o resultado (entre 2 e 12)
    /// </summary>
    public static int Roll2d6()
    {
        int dice1 = Random.Range(1, 7);
        int dice2 = Random.Range(1, 7);
        return dice1 + dice2;
    }

    /// <summary>
    /// Converte o resultado do 2d6 (2~12) para uma porcentagem (0~100)
    /// </summary>
    public static float RollToPercentage(int roll)
    {
        // 2 = 0%, 12 = 100%
        return ((roll - 2) / 10f) * 100f;
    }
}
