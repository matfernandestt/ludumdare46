using System;
using Rewired;
using UnityEngine;

public class FastForwardController : MonoBehaviour
{
    protected Player input;

    [SerializeField] private float BaseTimeSpeed = 1f;
    [SerializeField] private float FastTimeSpeed = 3f;
    [SerializeField] private GameObject FastForwardIcon;

    private bool castingFastForward;

    private const string ActionTag = "Action";
    private const string JumpTag = "Jump";
    private const string BackTag = "Back";

    private void Awake()
    {
        input = ReInput.players.GetPlayer(0);
        
        GameEvents.OnEnterEnemyPhase += EnableFastForward;
        GameEvents.OnPlayerCast += EnableFastForward;
        GameEvents.OnPlayerFinishCasting += DisableFastForward;
        GameEvents.OnTurnEnd += DisableFastForward;
        GameEvents.OnGameOver += DisableFastForward;
        GameEvents.OnBattleVictory += DisableFastForward;
        GameEvents.OnBeforeChangeScene += OnSceneChangeToDisableFastForward;

        FastForwardIcon.SetActive(false);
    }

    private void OnDestroy()
    {
        GameEvents.OnEnterEnemyPhase -= EnableFastForward;
        GameEvents.OnPlayerCast -= EnableFastForward;
        GameEvents.OnPlayerFinishCasting -= DisableFastForward;
        GameEvents.OnTurnEnd -= DisableFastForward;
        GameEvents.OnGameOver -= DisableFastForward;
        GameEvents.OnBattleVictory -= DisableFastForward;
        GameEvents.OnBeforeChangeScene -= OnSceneChangeToDisableFastForward;
    }
    
    private void OnSceneChangeToDisableFastForward(string obj)
    {
        DisableFastForward();
    }


    private void EnableFastForward()
    {
        castingFastForward = true;
    }

    private void DisableFastForward()
    {
        castingFastForward = false;
        Time.timeScale = BaseTimeSpeed;
        FastForwardIcon.SetActive(false);
    }

    private void Update()
    {
        if (!castingFastForward) return;
        FastForwardIcon.SetActive(Time.timeScale > BaseTimeSpeed);
        if (input.GetButton(ActionTag) ||
            input.GetButton(JumpTag) ||
            input.GetButton(BackTag))
        {
            Time.timeScale = FastTimeSpeed;
        }
        if (input.GetButtonUp(ActionTag) ||
            input.GetButtonUp(JumpTag) ||
            input.GetButtonUp(BackTag))
        {
            Time.timeScale = BaseTimeSpeed;
        }
    }
}
