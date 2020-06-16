using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private GameData GameData;
    [SerializeField] private PlayerBattleData PlayerData;

    private PlayerBattleData playerData;
    private BattleData battleData;
    private BattlePhase battlePhase;
    private int battleEnemyId;
    private Dictionary<int, string> DeadEnemies = new Dictionary<int, string>();
    
    private bool enteringBattle;

    public static bool EnteredGameFirstTime = true;

    public BattlePhase CurrentBattlePhase
    {
        get => battlePhase;
        set
        {
            battlePhase = value;
            switch (battlePhase)
            {
                case BattlePhase.SelectionPhase:
                    GameEvents.OnChangeBattlePhase?.Invoke();
                    GameEvents.OnEnterSelectionPhase?.Invoke();
                    break;
                case BattlePhase.ActionSelectionPhase:
                    GameEvents.OnChangeBattlePhase?.Invoke();
                    GameEvents.OnEnterSelectActionPhase?.Invoke();
                    break;
                case BattlePhase.TargetPhase:
                    GameEvents.OnChangeBattlePhase?.Invoke();
                    GameEvents.OnEnterTargetPhase?.Invoke();
                    break;
                case BattlePhase.ExecutePhase:
                    GameEvents.OnChangeBattlePhase?.Invoke();
                    GameEvents.OnEnterExecutePhase?.Invoke();
                    break;
                case BattlePhase.EnemyPhase:
                    GameEvents.OnChangeBattlePhase?.Invoke();
                    GameEvents.OnEnterEnemyPhase?.Invoke();
                    break;
            }
        }
    }

    private const string OverworldSceneTag = "OverWorld";
    private const string BattleSceneTag = "Battle";
    
    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        GameEvents.OnEnterBattle += OnEnterBattle;
    }

    private void OnDestroy()
    {
        GameEvents.OnEnterBattle -= OnEnterBattle;
    }
    
    public static void ResetPlayerHP(PlayerBattleData data)
    {
        if(EnteredGameFirstTime)
        {
            data.ResetHealth();
            EnteredGameFirstTime = false;
            GameEvents.OnUpdateUi?.Invoke();
        }
    }

    private void OnEnterBattle(PlayerBattleData playerBattleData, BattleData data)
    {
        enteringBattle = true;
        battleData = data;
        playerData = playerBattleData;

        IEnumerator WaitAnimationToChangeScene()
        {
            yield return new WaitWhile(() => instance.enteringBattle);
            GameEvents.OnBeforeChangeScene?.Invoke(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(BattleSceneTag);
        }

        StartCoroutine(WaitAnimationToChangeScene());
    }

    public static void LoadOverworld()
    {
        IEnumerator WaitFrame()
        {
            GameEvents.OnBeforeChangeScene?.Invoke(SceneManager.GetActiveScene().name);
            yield return null;
            SceneManager.LoadScene(OverworldSceneTag);
        }
        instance.StartCoroutine(WaitFrame());
    }

    public static void LoadOverworldAfterPitfall()
    {
        EnteredGameFirstTime = true;
        ResetEnemiesDefeated();
        SceneManager.LoadScene(OverworldSceneTag);
    }

    public static void OnEnteredBattle()
    {
        instance.enteringBattle = false;
    }

    public static void ResetBattlePhase()
    {
        instance.CurrentBattlePhase = BattlePhase.SelectionPhase;
    }

    public static void SetManuallyBattlePhase(BattlePhase phase)
    {
        instance.CurrentBattlePhase = phase;
    }

    public static BattlePhase GetBattlePhase()
    {
        return instance.battlePhase;
    }

    public static Dictionary<int, string> GetEnemyDictionary()
    {
        return instance.DeadEnemies;
    }

    public static void SetEnemyDictionary(int key, string value)
    {
        instance.DeadEnemies.Add(key, value);
    }

    public static void ResetEnemiesDefeated()
    {
        instance.DeadEnemies.Clear();
    }

    public static void GetPreviousBattlePhase()
    {
        var currentPhaseId = (int)instance.battlePhase;
        currentPhaseId--;
        if (currentPhaseId >= 0)
            instance.CurrentBattlePhase = (BattlePhase)currentPhaseId;
    }

    public static void SetBattleEnemyId(int id)
    {
        instance.battleEnemyId = id;
    }

    public static int GetBattleEnemyId()
    {
        return instance.battleEnemyId;
    }
    
    public static void GetNextBattlePhase()
    {
        var currentPhaseId = (int)instance.battlePhase;
        currentPhaseId++;
        if (currentPhaseId > (int)BattlePhase.EnemyPhase)
            currentPhaseId = 0;
        instance.CurrentBattlePhase = (BattlePhase)currentPhaseId;
    }

    public static bool GetBattleEntranceStatus()
    {
        return instance.enteringBattle;
    }

    public static GameData GetGameData()
    {
        return instance.GameData;
    }

    public static BattleData GetBattleData()
    {
        return instance.battleData;
    }

    public static PlayerBattleData GetPlayerBattleData()
    {
        return instance.playerData;
    }

    public static string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    
    public static Vector3 GetRelativeDirection(Vector3 thisPos, Vector3 otherPos)
    {
        Vector3 direction = new Vector3();
        direction = (otherPos - thisPos).normalized;
        return direction;
    }
}
