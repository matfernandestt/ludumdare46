using System;
using TMPro;
using UnityEngine;

public class OverworldUI : MonoBehaviour
{
    [SerializeField] private GameObject BattleEnteringEffect;
    [SerializeField] private PlayerBattleData CurrentPlayerData;
    [SerializeField] private TextMeshProUGUI PlayerMaxHealth;
    [SerializeField] private TextMeshProUGUI PlayerHealth;

    [SerializeField] private AudioSource OverworldSrc;
    [SerializeField] private AudioSource EnterBattleSrc;

    private void Awake()
    {
        GameEvents.OnPrepareToEnterBattle += OnPrepareToEnterBattle;
        GameEvents.OnUpdateUi += SetupHud;
        
        OverworldSrc.Play();
        SetupHud();
    }

    private void OnDestroy()
    {
        GameEvents.OnPrepareToEnterBattle -= OnPrepareToEnterBattle;
        GameEvents.OnUpdateUi -= SetupHud;
    }

    private void OnPrepareToEnterBattle()
    {
        OverworldSrc.Stop();
        EnterBattleSrc.Play();
        
        BattleEnteringEffect.SetActive(true);
    }

    private void SetupHud()
    {
        PlayerMaxHealth.text = CurrentPlayerData.MaxHealthPoints.ToString();
        PlayerHealth.text = CurrentPlayerData.HealthPoints.ToString();
    }
}
