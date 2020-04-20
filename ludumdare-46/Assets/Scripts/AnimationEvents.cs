using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Action OnFinishAnimation;
    
    public void OnFinishIntroAnimation()
    {
        GameManager.OnEnteredBattle();
    }

    public void OnFinishOpeningAnimation()
    {
        gameObject.SetActive(false);
    }

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }

    public void FinishedAnimation()
    {
        OnFinishAnimation?.Invoke();
    }
}
