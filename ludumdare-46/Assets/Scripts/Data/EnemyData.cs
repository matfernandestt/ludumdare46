using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game Design/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public BattleEnemy EnemyPrefab;
    public EnemyStatus EnemyStatus;
}
