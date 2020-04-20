using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SurroundingDetectorAI : MonoBehaviour
{
    [SerializeField] private BaseMovementAI AIController;
    [SerializeField] private BattleData battleData;

    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private IEnumerator Start()
    {
        col.enabled = false;
        yield return new WaitForSeconds(2f);
        col.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var knockbackable = other.GetComponent<IKnockbackable>();
        if (knockbackable != null)
        {
            knockbackable.AddImpact(GameManager.GetRelativeDirection(transform.position, other.transform.position), 600);
        }
        
        var player = other.GetComponent<BattleDetector>();
        if (player != null && !GameManager.GetBattleEntranceStatus())
        {
            GameManager.SetBattleEnemyId(AIController.EnemyId);
            GameEvents.OnPrepareToEnterBattle?.Invoke();
            GameEvents.OnEnterBattle?.Invoke(player.SelfBattleData, battleData);
        }
    }
}
