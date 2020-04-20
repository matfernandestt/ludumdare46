using System;

public static class GameEvents
{
    public static Action<string> OnBeforeChangeScene;
    public static Action<string> OnLoadSceneController;
    
    public static Action<PlayerBattleData, BattleData> OnEnterBattle;
    public static Action OnPrepareToEnterBattle;

    public static Action OnSelectAttack;
    public static Action OnSelectItem;
    public static Action OnSelectExit;
    
    public static Action OnChangeTargetedEnemyUp;
    public static Action OnChangeTargetedEnemyDown;
    public static Action OnSelectTargetedEnemy;

    public static Action OnChangeBattlePhase;
    public static Action OnEnterSelectionPhase;
    public static Action OnEnterTargetPhase;
    public static Action OnEnterSelectActionPhase;
    public static Action OnEnterExecutePhase;
    public static Action OnEnterEnemyPhase;

    public static Action OnSelectActionUp;
    public static Action OnSelectActionDown;
    public static Action OnConfirmAction;
    public static Action<AttackOptionSelected> OnChooseOption;

    public static Action<BattleEnemySpot> OnStartPlayerAction;
    public static Action OnPlayerUseAttack;
    public static Action OnPlayerUseMagic;
    public static Action OnPlayerUseGuard;

    public static Action<int> OnEnemyReceiveDamage;
    public static Action<int> OnPlayerReceiveDamage;
    public static Action OnEnemyDeath;
    public static Action OnBattleVictory;
    public static Action OnGameOver;

    public static Action OnSpeedRush;
    public static Action OnUpdateUi;
    public static Action<int> OnPlayerHeal;

    public static Action OnGoBack;
}
