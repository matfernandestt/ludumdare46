using System;
using System.Collections;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup SelectionPhaseUi;
    [SerializeField] private AttackSelectionUI AttackSelectionMenu;
    [Header("Selection")]
    [SerializeField] private Animator SelectAttackButton;
    [SerializeField] private Animator SelectItemButton;
    [SerializeField] private Animator SelectExitButton;

    private Coroutine fadeOutCoroutine;
    private WaitForSeconds waitTime = new WaitForSeconds(.01f); 
    
    private static readonly int Selection = Animator.StringToHash("Selection");

    private void Awake()
    {
        GameEvents.OnEnterSelectionPhase += OnEnterSelectionPhase;
        GameEvents.OnEnterSelectActionPhase += OnEnterActionSelecitonPhase;
        GameEvents.OnEnterTargetPhase += OnEnterTargetPhase;
        GameEvents.OnEnterExecutePhase += OnEnterExecutePhase;
        GameEvents.OnEnterEnemyPhase += OnEnterEnemyPhase;
        
        GameEvents.OnSelectAttack += OnSelectAttack;
        GameEvents.OnSelectItem += OnSelectItem;
        GameEvents.OnSelectExit += OnSelectExit;

        GameEvents.OnBattleVictory += OnBattleVictory;
    }

    private void OnDestroy()
    {
        GameEvents.OnEnterSelectionPhase -= OnEnterSelectionPhase;
        GameEvents.OnEnterSelectActionPhase -= OnEnterActionSelecitonPhase;
        GameEvents.OnEnterTargetPhase -= OnEnterTargetPhase;
        GameEvents.OnEnterExecutePhase -= OnEnterExecutePhase;
        GameEvents.OnEnterEnemyPhase -= OnEnterEnemyPhase;
        
        GameEvents.OnSelectAttack -= OnSelectAttack;
        GameEvents.OnSelectItem -= OnSelectItem;
        GameEvents.OnSelectExit -= OnSelectExit;
        
        GameEvents.OnBattleVictory -= OnBattleVictory;
    }

    private void OnSelectAttack()
    {
        SelectAttackButton.SetTrigger(Selection);
        FadeOutSelectionPhaseUi();
    }
    
    private void OnSelectItem()
    {
        SelectItemButton.SetTrigger(Selection);
    }
    
    private void OnSelectExit()
    {
        SelectExitButton.SetTrigger(Selection);
    }

    private void OnEnterSelectionPhase()
    {
        FadeInSelectionPhaseUi();
        AttackSelectionMenu.gameObject.SetActive(false);
    }

    private void OnEnterActionSelecitonPhase()
    {
        AttackSelectionMenu.gameObject.SetActive(true);
    }
    
    private void OnEnterTargetPhase()
    {
        AttackSelectionMenu.gameObject.SetActive(false);
    }
    
    private void OnEnterExecutePhase()
    {
        
    }
    
    private void OnEnterEnemyPhase()
    {
        
    }

    private void FadeOutSelectionPhaseUi()
    {
        IEnumerator FadeOut()
        {
            var progress = 0f;
            while (progress < 1f)
            {
                progress += .1f;
                SelectionPhaseUi.alpha = 1 - progress;
                yield return waitTime;
            }
        }
        if(fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);
        fadeOutCoroutine = StartCoroutine(FadeOut());
    }
    
    private void FadeInSelectionPhaseUi()
    {
        IEnumerator FadeIn()
        {
            var progress = 0f;
            while (progress < 1f)
            {
                progress += .1f;
                SelectionPhaseUi.alpha = progress;
                yield return waitTime;
            }
        }
        if(fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);
        fadeOutCoroutine = StartCoroutine(FadeIn());
    }
    
    private void OnBattleVictory()
    {
        SelectionPhaseUi.gameObject.SetActive(false);
        AttackSelectionMenu.gameObject.SetActive(false);
    }
}
