using UnityEngine;
using System.Collections;
public class MapProgressionManager : MonoBehaviour
{
    [Header("References")]
    public Transform progressionPoints;     
    public Transform miniPlayer;
    [Header("Movement")]
    public float moveSpeed = 3f;


    public void MoveToProgressionPoint(int index)
    {
        if (progressionPoints == null || miniPlayer == null) return;
        if (index < 0 || index >= progressionPoints.childCount)
        {
            Debug.LogWarning($"Index {index} inválido para progressionPoints");
            return;
        }

        Transform targetPoint = progressionPoints.GetChild(index);
        Debug.Log("=== DEBUG POSIÇÃO ===");
        Debug.Log($"Index: {index}");
        Debug.Log($"Target World Position: {targetPoint.position}");
        Debug.Log($"MiniPlayer World Position (antes): {miniPlayer.position}");
        Debug.Log($"Target Local Position: {targetPoint.localPosition}");
        Debug.Log($"Parent do Target Position: {targetPoint.parent.position}");

        // Se for o ponto 0 (começo de level)  teleporta
        if (index == 0)
        {
            miniPlayer.position = targetPoint.position;
        }
        else
        {
            // Nos outros pontos  move suavemente
            StartCoroutine(MoveSmoothly(targetPoint.position));
        }
    }

    IEnumerator MoveSmoothly(Vector3 targetPosition)
    {
        while (Vector3.Distance(miniPlayer.position, targetPosition) > 0.05f)
        {
            miniPlayer.position = Vector3.MoveTowards(
                miniPlayer.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Garante que chegou exatamente no ponto
        miniPlayer.position = targetPosition;
    }
}
