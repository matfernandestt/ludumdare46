using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BattleInfoUI : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    
    [SerializeField] private TextMeshProUGUI PlayerMaxHealth;
    [SerializeField] private TextMeshProUGUI PlayerHealth;
    [SerializeField] private TextMeshProUGUI TurnsPassed;

    [SerializeField] private GameObject SpeedLines;
    [SerializeField] private GameObject SpeedRush;

    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private AnimationEvents GameOverPanelAnimationEvents;

    private int turn = 1;
    private bool anyButtonToReset;

    private void Start()
    {
        GameEvents.OnUpdateUi += UpdateUi;

        GameEvents.OnSpeedRush += SpeedRushing;
        GameEvents.OnTurnEnd += OnTurnEnd;
        GameEvents.OnGameOver += OnGameOver;

        GameOverPanelAnimationEvents.OnFinishAnimation += OnFinishedGameOverAnimation;

        UpdateUi();
    }

    private void OnDestroy()
    {
        GameEvents.OnUpdateUi -= UpdateUi;
        
        GameEvents.OnSpeedRush -= SpeedRushing;
        GameEvents.OnTurnEnd -= OnTurnEnd;
        GameEvents.OnGameOver -= OnGameOver;
        
        GameOverPanelAnimationEvents.OnFinishAnimation -= OnFinishedGameOverAnimation;
    }

    private void UpdateUi()
    {
        PlayerHealth.text = battleManager.PlayerInBattleData.HealthPoints.ToString();
        PlayerMaxHealth.text = battleManager.PlayerInBattleData.MaxHealthPoints.ToString();
        TurnsPassed.text = $"{turn}";
    }

    private void OnTurnEnd()
    {
        turn++;
        GameEvents.OnUpdateUi?.Invoke();
    }

    private void SpeedRushing()
    {
        IEnumerator Activate()
        {
            SpeedRush.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            SpeedRush.SetActive(false);
        }

        StartCoroutine(Activate());
    }

    private void OnGameOver()
    {
        GameOverPanel.SetActive(true);
    }

    private void OnFinishedGameOverAnimation()
    {
        anyButtonToReset = true;
    }

    public void GameOverReset()
    {
        GameManager.EnteredGameFirstTime = true;
        
        SceneDataKeeper.ResetSavedData();
        GameManager.ResetEnemiesDefeated();
        GameManager.LoadOverworld();
    }

    private void Update()
    {
        if (!anyButtonToReset) return;
        if (Input.anyKeyDown)
        {
            GameOverReset();
        }
    }
}
