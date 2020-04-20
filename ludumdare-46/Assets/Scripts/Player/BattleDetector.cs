using System;
using UnityEngine;

public class BattleDetector : MonoBehaviour
{
    [SerializeField] private PlayerBattleData playerBattleData;

    public PlayerBattleData SelfBattleData => playerBattleData;

    private void Start()
    {
        GameManager.ResetPlayerHP(playerBattleData);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DeathPit"))
        {
            GameManager.LoadOverworldAfterPitfall();
        }
    }
}
