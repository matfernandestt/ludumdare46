using System;
using UnityEngine;

public class TargetChaser : MonoBehaviour
{
    [SerializeField] private BaseMovementAI EnemyAi;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<BattleDetector>();
        if (player != null)
        {
            if (EnemyAi.CurrentBehaviour != EnemyBehaviour.Chasing)
            {
                AudioManager.EnemyNoticeSFX();
                EnemyAi.StartChase(other.transform);
            }
        }
    }
}
