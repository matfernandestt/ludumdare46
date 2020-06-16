using System;
using System.Collections;
using Rewired;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    protected Player input;

    [SerializeField] private Animator ThisAnim;
    [SerializeField] private DamageVisualizer DamageUi;
    [SerializeField] private GameObject GuardEffect;
    [SerializeField] private AudioSource GuardSFX;
    
    private bool lockInputs;
    private Vector3 originalPlayerPosition;
    private AttackOptionSelected attackOption;
    private Coroutine actionMoveCoroutine;
    private bool onGuard;
    private bool gameEnded;
    
    private static readonly int BasicMovement = Animator.StringToHash("BasicMovement");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Death");

    private void Awake()
    {
        input = ReInput.players.GetPlayer(0);

        GameEvents.OnChooseOption += OnAttackOptionConfirmed;
        GameEvents.OnStartPlayerAction += MoveToAttackEnemy;
        GameEvents.OnBattleVictory += OnBattleVictory;
        GameEvents.OnPlayerHeal += OnHeal;
        GameEvents.OnPlayerReceiveDamage += OnReceiveDamage;
        GameEvents.OnGameOver += OnGameOver;
        
        originalPlayerPosition = transform.position;
    }

    private void OnDestroy()
    {
        GameEvents.OnChooseOption -= OnAttackOptionConfirmed;
        GameEvents.OnStartPlayerAction -= MoveToAttackEnemy;
        GameEvents.OnBattleVictory -= OnBattleVictory;
        GameEvents.OnPlayerHeal -= OnHeal;
        GameEvents.OnPlayerReceiveDamage -= OnReceiveDamage;
        GameEvents.OnGameOver -= OnGameOver;
    }

    private void Update()
    {
        if (lockInputs) return;
        switch (GameManager.GetBattlePhase())
        {
            case BattlePhase.SelectionPhase:
                SelectAttack();
                SelectItem();
                SelectExit();
                break;
            case BattlePhase.ActionSelectionPhase:
                SelectActionUp();
                SelectActionDown();
                OnConfirmAction();
                break;
            case BattlePhase.TargetPhase:
                TargetSelectionUp();
                TargetSelectionDown();
                SelectEnemy();
                break;
            case BattlePhase.ExecutePhase:
                break;
            case BattlePhase.EnemyPhase:
                break;
        }

        if (GameManager.GetBattlePhase() == BattlePhase.ExecutePhase ||
            GameManager.GetBattlePhase() == BattlePhase.EnemyPhase ||
            GameManager.GetBattlePhase() == BattlePhase.SelectionPhase) return;
        GoBack();
    }

    private void GoBack()
    {
        if (input.GetButtonDown("Back"))
        {
            GameEvents.OnGoBack?.Invoke();
            GameManager.GetPreviousBattlePhase();
            AudioManager.BackSFX();
        }
    }

    private void SelectAttack()
    {
        if (input.GetButtonDown("BattleAttack"))
        {
            GameEvents.OnSelectAttack?.Invoke();
            GameManager.GetNextBattlePhase();
            AudioManager.SelectionBlipSFX();
        }
    }

    private void SelectItem()
    {
        if (input.GetButtonDown("BattleItem"))
        {
            GameEvents.OnSelectItem?.Invoke();
            AudioManager.SelectionBlipSFX();
        }
    }

    private void SelectExit()
    {
        if (input.GetButtonDown("BattleExit"))
        {
            GameEvents.OnSelectExit?.Invoke();
            AudioManager.SelectionBlipSFX();
            GameManager.LoadOverworld();
        }
    }

    private void TargetSelectionUp()
    {
        if (input.GetButtonDown("SelectUp") || input.GetButtonDown("SelectLeft"))
        {
            GameEvents.OnChangeTargetedEnemyUp?.Invoke();
            AudioManager.SelectionBlipSFX();
        }
    }

    private void TargetSelectionDown()
    {
        if (input.GetButtonDown("SelectDown") || input.GetButtonDown("SelectRight"))
        {
            GameEvents.OnChangeTargetedEnemyDown?.Invoke();
            AudioManager.SelectionBlipSFX();
        }
    }
    
    private void SelectActionUp()
    {
        if (input.GetButtonDown("SelectUp"))
        {
            GameEvents.OnSelectActionUp?.Invoke();
            AudioManager.SelectionBlipSFX();
        }
    }
    
    private void SelectActionDown()
    {
        if (input.GetButtonDown("SelectDown"))
        {
            GameEvents.OnSelectActionDown?.Invoke();
            AudioManager.SelectionBlipSFX();
        }
    }

    private void OnConfirmAction()
    {
        if (input.GetButtonDown("Action") || input.GetButtonDown("Jump"))
        {
            GameEvents.OnConfirmAction?.Invoke();
            GameManager.GetNextBattlePhase();
            AudioManager.SelectionBlipSFX();
        }
    }

    private void SelectEnemy()
    {
        if (input.GetButtonDown("Action") || input.GetButtonDown("Jump"))
        {
            GameEvents.OnSelectTargetedEnemy?.Invoke();
            GameManager.GetNextBattlePhase();
            AudioManager.SelectionBlipSFX();
        }
    }

    private void OnAttackOptionConfirmed(AttackOptionSelected optionSelected)
    {
        attackOption = optionSelected;
        if(attackOption == AttackOptionSelected.Guard)
            GameManager.GetNextBattlePhase();
    }
    
    private void OnHeal(int healQuanity)
    {
        DamageUi.ShowHealText(healQuanity);
    }

    private void OnReceiveDamage(int dmg)
    {
        if (onGuard)
        {
            onGuard = false;
            GuardEffect.SetActive(false);
            GuardSFX.Play();
            dmg = 0;
        }
        DamageUi.ShowDamageText(dmg);
    }

    private void MoveToAttackEnemy(BattleEnemySpot battleSpot)
    {
        if(actionMoveCoroutine != null)
            StopCoroutine(actionMoveCoroutine);
        switch (attackOption)
        {
            case AttackOptionSelected.Attack:
                actionMoveCoroutine = StartCoroutine(StartAttacking(battleSpot));
                break;
            case AttackOptionSelected.Magic:
                actionMoveCoroutine = StartCoroutine(StartCastingMagic(battleSpot));
                break;
            case AttackOptionSelected.Guard:
                actionMoveCoroutine = StartCoroutine(StartGuardPose(battleSpot));
                break;
        }
    }
    
    private IEnumerator StartAttacking(BattleEnemySpot enemySpot)
    {
        GameEvents.OnSpeedRush?.Invoke();
        var attackPosition = enemySpot.PlayerActionSpot.position;
        var speed = 4f;
        lockInputs = true;
        ThisAnim.SetFloat(BasicMovement, 1);
        float step = (speed / (transform.position - attackPosition).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= .3f)
        {
            t += step;
            transform.position = Vector3.Lerp(transform.position, attackPosition, t);
            if(t > .3f)
                ThisAnim.SetFloat(BasicMovement, 0);
            yield return new WaitForFixedUpdate();
        }
        transform.position = attackPosition;
        transform.position = attackPosition;
        ThisAnim.SetTrigger(Attack);
        AudioManager.AttackSFX();
        yield return new WaitForSeconds(.5f);
        GameEvents.OnPlayerUseAttack?.Invoke();
        yield return new WaitForSeconds(1f);
        GameEvents.OnPlayerCast?.Invoke();
        ThisAnim.SetFloat(BasicMovement, 1);
        speed = 8f;
        t = 0;
        while (t <= .3f)
        {
            t += step;
            transform.position = Vector3.Lerp(transform.position, originalPlayerPosition, t);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 180, 0), t * 5f);
            if(t > .3f)
                ThisAnim.SetFloat(BasicMovement, 0);
            yield return new WaitForFixedUpdate();
        }
        transform.position = originalPlayerPosition;
        speed = 10f;
        t = 0;
        while (t <= .2f)
        {
            t += step;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 0), t * 5f);
            yield return new WaitForFixedUpdate();
        }
        transform.eulerAngles = new Vector3(0, 0, 0);
        lockInputs = false;
        GameEvents.OnPlayerFinishCasting.Invoke();
        GameManager.GetNextBattlePhase();
    }
    
    private IEnumerator StartCastingMagic(BattleEnemySpot enemySpot)
    {
        lockInputs = true;
        yield return new WaitForSeconds(.5f);
        AudioManager.AttackSFX();
        GameEvents.OnPlayerUseMagic?.Invoke();
        yield return new WaitForSeconds(.5f);
        lockInputs = false;
        GameEvents.OnPlayerFinishCasting.Invoke();
        GameManager.GetNextBattlePhase();
    }
    
    private IEnumerator StartGuardPose(BattleEnemySpot enemySpot)
    {
        GameEvents.OnPlayerReceiveDamage?.Invoke(1);
        GameEvents.OnUpdateUi?.Invoke();
        lockInputs = true;
        yield return new WaitForSeconds(.5f);
        if (gameEnded) yield break;
        GameEvents.OnPlayerUseGuard?.Invoke();
        onGuard = true;
        GuardEffect.SetActive(true);
        GuardSFX.Play();
        yield return new WaitForSeconds(.5f);
        lockInputs = false;
        GameEvents.OnPlayerFinishCasting.Invoke();
        GameManager.ResetBattlePhase();
        //GameManager.GetNextBattlePhase();
    }

    private void OnBattleVictory()
    {
        lockInputs = true;
    }

    private void OnGameOver()
    {
        gameEnded = true;
        ThisAnim.SetTrigger(Death);
    }
}
