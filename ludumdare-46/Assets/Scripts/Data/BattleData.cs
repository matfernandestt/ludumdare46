using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "Game Design/Battle Data")]
public class BattleData : ScriptableObject
{
    public EnemyData[] Enemies;
}
