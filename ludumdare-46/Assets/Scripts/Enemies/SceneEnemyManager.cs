using System.Collections.Generic;
using UnityEngine;

public class SceneEnemyManager : MonoBehaviour
{
    public List<EnemyIdentification> EnemiesId = new List<EnemyIdentification>();

    private void Awake()
    {
        var scene = GameManager.GetCurrentSceneName();
        foreach (var enemy in EnemiesId)
        {
            enemy.EnemyGameObject.EnemyId = enemy.EnemyId;
        }

        if (GameManager.GetEnemyDictionary().Count > 0)
        {
            foreach (var enemy in GameManager.GetEnemyDictionary().Keys)
            {
                EnemiesId[enemy].EnemyGameObject.InstantDeath();
            }
        }
    }
}

[System.Serializable]
public class EnemyIdentification
{
    public int EnemyId;
    public BaseMovementAI EnemyGameObject;
}
