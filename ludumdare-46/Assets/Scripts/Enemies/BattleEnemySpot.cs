using System;
using UnityEngine;

public class BattleEnemySpot : MonoBehaviour
{
    [SerializeField] private Transform PlayerActionPosition;
    [SerializeField] private DamageVisualizer dmgVisualizer;
    
    [Header("Filled in runtime")]
    public BattleEnemy LinkedEnemy;
    
    public Transform PlayerActionSpot => PlayerActionPosition;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, .5f);
        if (PlayerActionPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(PlayerActionPosition.position, new Vector3(.5f, .2f, .5f));
        }
    }

    public void ShowDamage(int dmg)
    {
        dmgVisualizer.ShowDamageText(dmg);
    }
}
