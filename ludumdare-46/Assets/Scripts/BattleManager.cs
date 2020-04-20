using System.Collections;
using System.Linq;
using UnityEngine;

public enum BattlePhase
{
    SelectionPhase = 0,
    ActionSelectionPhase = 1,
    TargetPhase = 2,
    ExecutePhase = 3,
    EnemyPhase = 4
}

public class BattleManager : MonoBehaviour
{
    [SerializeField] private AudioSource BattleSrc;
    [SerializeField] private AudioSource VictorySrc;
    [SerializeField] private AudioSource GameOverSrc;
    [SerializeField] private BattleEnemySpot[] EnemySpots;
    [SerializeField] private Transform PlayerDamageSpot;
    
    [SerializeField] private BattleEnemy DeadEnemyPreset;
    
    private PlayerBattleData playerInBattle;
    private BattleData thisBattle;
    private BattleEnemy enemyTarget;
    
    private int targetedEnemyId = 0;
    private bool playerOnGuard;
    private bool gameEnded;
    private bool playerDied;
    
    public PlayerBattleData PlayerInBattleData => playerInBattle;

    private void Awake()
    {
        GameEvents.OnEnterSelectionPhase += OnEnterSelectionPhase;
        GameEvents.OnEnterTargetPhase += OnEnterTargetPhase;
        GameEvents.OnEnterSelectActionPhase += OnEnterActionSelectionPhase;
        GameEvents.OnEnterExecutePhase += OnEnterExecutionPhase;
        GameEvents.OnEnterEnemyPhase += OnEnterEnemyPhase;

        GameEvents.OnEnterTargetPhase += SelectEnemy;
        GameEvents.OnChangeTargetedEnemyUp += EnemySelectionUp;
        GameEvents.OnChangeTargetedEnemyDown += EnemySelectionDown;
        GameEvents.OnSelectTargetedEnemy += TargetedEnemy;
        GameEvents.OnPlayerUseAttack += DealEnemyDamage;
        GameEvents.OnPlayerUseMagic += DealEnemyMagicDamage;
        GameEvents.OnEnemyDeath += EnemyReselection;
        GameEvents.OnPlayerReceiveDamage += OnPlayerReceiveDamage;
        GameEvents.OnPlayerUseGuard += SetPlayerOnGuard;
        GameEvents.OnGameOver += OnGameOver;
        
        thisBattle = GameManager.GetBattleData();
        playerInBattle = GameManager.GetPlayerBattleData();
        
        for (var i = 0; i < EnemySpots.Length; i++)
        {
            if (i < thisBattle.Enemies.Length)
            {
                var enemy = thisBattle.Enemies[i].EnemyPrefab;
                var bEnemy = Instantiate(enemy, EnemySpots[i].transform.position, Quaternion.identity);
                bEnemy.SetStatus(enemy.data.EnemyStatus);
                EnemySpots[i].LinkedEnemy = bEnemy;
            }
            else
            {
                var bEnemy = Instantiate(DeadEnemyPreset, EnemySpots[i].transform.position, Quaternion.identity);
                EnemySpots[i].LinkedEnemy = bEnemy;
            }
        }
        targetedEnemyId = 0;
        GameManager.ResetBattlePhase();
        
        BattleSrc.Play();
    }

    private void OnDestroy()
    {
        GameEvents.OnEnterSelectionPhase -= OnEnterSelectionPhase;
        GameEvents.OnEnterTargetPhase -= OnEnterTargetPhase;
        GameEvents.OnEnterSelectActionPhase -= OnEnterActionSelectionPhase;
        GameEvents.OnEnterExecutePhase -= OnEnterExecutionPhase;
        GameEvents.OnEnterEnemyPhase -= OnEnterEnemyPhase;
        
        GameEvents.OnEnterTargetPhase -= SelectEnemy;
        GameEvents.OnChangeTargetedEnemyUp -= EnemySelectionUp;
        GameEvents.OnChangeTargetedEnemyDown -= EnemySelectionDown;
        GameEvents.OnSelectTargetedEnemy -= TargetedEnemy;
        GameEvents.OnPlayerUseAttack -= DealEnemyDamage;
        GameEvents.OnPlayerUseMagic -= DealEnemyMagicDamage;
        GameEvents.OnEnemyDeath -= EnemyReselection;
        GameEvents.OnPlayerReceiveDamage -= OnPlayerReceiveDamage;
        GameEvents.OnPlayerUseGuard -= SetPlayerOnGuard;
        GameEvents.OnGameOver -= OnGameOver;
    }

    private void OnEnterSelectionPhase()
    {
        UnselectAllEnemies();
    }
    
    private void OnEnterActionSelectionPhase()
    {
        UnselectAllEnemies();
    }
    
    private void OnEnterTargetPhase()
    {
        
    }

    private void OnEnterExecutionPhase()
    {
        UnselectAllEnemies();
        GameEvents.OnStartPlayerAction?.Invoke(EnemySpots[targetedEnemyId]);
    }
    
    private void OnEnterEnemyPhase()
    {
        UnselectAllEnemies();

        IEnumerator EnemyAttack()
        {
            foreach (var enemy in EnemySpots)
            {
                if (!enemy.LinkedEnemy.Dead)
                {
                    enemy.LinkedEnemy.Attack(PlayerDamageSpot);
                    yield return new WaitWhile(() => enemy.LinkedEnemy.Attacking || playerDied);
                }
            }
            if(!playerDied)
                GameManager.GetNextBattlePhase();
        }

        StartCoroutine(EnemyAttack());
    }

    private void OnGameOver()
    {
        playerDied = true;
        PlayerDefeat();
    }

    private void EnemySelectionDown()
    {
        while (true)
        {
            targetedEnemyId--;
            if (targetedEnemyId < 0) targetedEnemyId = EnemySpots.Length - 1;
            if (EnemySpots[targetedEnemyId].LinkedEnemy.Dead)
                continue;
            SelectEnemy();
            break;
        }
    }

    private void EnemySelectionUp()
    {
        while (true)
        {
            targetedEnemyId++;
            if (targetedEnemyId > EnemySpots.Length - 1) targetedEnemyId = 0;
            if (EnemySpots[targetedEnemyId].LinkedEnemy.Dead)
                continue;
            SelectEnemy();
            break;
        }
    }

    private void EnemyReselection()
    {
        while (!CheckBattleStatus())
        {
            targetedEnemyId--;
            if (targetedEnemyId < 0) targetedEnemyId = EnemySpots.Length - 1;
            if (EnemySpots[targetedEnemyId].LinkedEnemy.Dead)
                continue;
            UnselectAllEnemies();
            break;
        }
    }

    private void SelectEnemy()
    {
        UnselectAllEnemies();
        EnemySpots[targetedEnemyId].LinkedEnemy.SetSelected();
    }

    private void UnselectAllEnemies()
    {
        foreach (var enemy in EnemySpots)
        {
            enemy.LinkedEnemy.Unselect();
        }
    }

    private void TargetedEnemy()
    {
        enemyTarget = EnemySpots[targetedEnemyId].LinkedEnemy;
        UnselectAllEnemies();
    }

    private void SetPlayerOnGuard()
    {
        playerOnGuard = true;
    }

    private void OnPlayerReceiveDamage(int dmg)
    {
        if (playerOnGuard)
        {
            playerOnGuard = false;
            dmg = 0;
        }
        playerInBattle.ReceiveDamage(dmg);
    }

    private void DealEnemyDamage()
    {
        EnemySpots[targetedEnemyId].ShowDamage(playerInBattle.Strength);
        EnemySpots[targetedEnemyId].LinkedEnemy.ReceiveDamage(playerInBattle.Strength);
        CheckBattleStatus();
    }
    
    private void DealEnemyMagicDamage()
    {
        var dmg = (int) (playerInBattle.Strength / 2);
        playerInBattle.Heal(5);
        GameEvents.OnPlayerHeal?.Invoke(5);
        EnemySpots[targetedEnemyId].ShowDamage(dmg);
        EnemySpots[targetedEnemyId].LinkedEnemy.ReceiveDamage(dmg);
        CheckBattleStatus();
        GameEvents.OnUpdateUi?.Invoke();
    }

    private void DealPlayerDamage(int damage)
    {
        playerInBattle.ReceiveDamage(damage);
    }

    private bool CheckBattleStatus()
    {
        var enemiesDead = EnemySpots.Count(enemy => enemy.LinkedEnemy.Dead);
        if (enemiesDead >= EnemySpots.Length && !gameEnded)
        {
            PlayerVictory();
        }
        return enemiesDead >= EnemySpots.Length;
    }

    private void PlayerVictory()
    {
        BattleSrc.Stop();
        VictorySrc.Play();
        gameEnded = true;
        GameManager.SetEnemyDictionary(GameManager.GetBattleEnemyId(), "Overworld");
        GameEvents.OnBattleVictory?.Invoke();
        StartCoroutine(WaitSecondsToReturnToOverworld());
    }

    private void PlayerDefeat()
    {
        BattleSrc.Stop();
        GameOverSrc.Play();
    }

    private IEnumerator WaitSecondsToReturnToOverworld()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.LoadOverworld();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (PlayerDamageSpot != null)
            Gizmos.DrawWireCube(PlayerDamageSpot.position, new Vector3(.5f, .2f, .5f));
    }
}