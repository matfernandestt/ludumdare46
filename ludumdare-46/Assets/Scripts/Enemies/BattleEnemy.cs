using System;
using System.Collections;
using UnityEngine;

public class BattleEnemy : MonoBehaviour
{
    public EnemyData data;
    public bool Attacking;
    public bool Dead;

    [SerializeField] private GameObject SelectionMark;
    [SerializeField] private SpriteRenderer EnemySprite;
    [SerializeField] private GameObject EnemyShadow;

    private int hp;
    private int str;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void ReceiveDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
            Death();
    }

    public void SetStatus(EnemyStatus status)
    {
        hp = status.HealthPoints;
        str = status.Strength;
    }

    public void SetSelected()
    {
        if(SelectionMark != null)
            SelectionMark.SetActive(true);
    }

    public void Unselect()
    {
        if(SelectionMark != null)
            SelectionMark.SetActive(false);
    }

    private void Death()
    {
        FadeSprite();
        EnemyShadow.SetActive(false);
        Dead = true;
        GameEvents.OnEnemyDeath?.Invoke();
    }

    private void FadeSprite()
    {
        IEnumerator Fade()
        {
            var speed = 4f;
            float step = (speed / 1) * Time.fixedDeltaTime;
            float t = 0;
            while (t <= 1.0f)
            {
                t += step;
                EnemySprite.color = new Color(EnemySprite.color.r, EnemySprite.color.g, EnemySprite.color.b, 1 - t);
                yield return new WaitForFixedUpdate();
            }
        }

        StartCoroutine(Fade());
    }

    public void Attack(Transform playerDamageSpot)
    {
        Attacking = true;
        GameEvents.OnSpeedRush?.Invoke();

        IEnumerator StartAttacking()
        {
            var attackPosition = playerDamageSpot.position;
            var speed = 4f;
            float step = (speed / (transform.position - attackPosition).magnitude) * Time.fixedDeltaTime;
            float t = 0;
            while (t <= .3f)
            {
                t += step;
                transform.position = Vector3.Lerp(transform.position, attackPosition, t);
                yield return new WaitForFixedUpdate();
            }
            transform.position = attackPosition;
            AudioManager.AttackSFX();
            yield return new WaitForSeconds(.5f);
            GameEvents.OnPlayerReceiveDamage?.Invoke(str);
            GameEvents.OnUpdateUi?.Invoke();
            yield return new WaitForSeconds(1f);
            t = 0;
            while (t <= .3f)
            {
                t += step;
                transform.position = Vector3.Lerp(transform.position, originalPosition, t);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 180, 0), t * 5f);
                yield return new WaitForFixedUpdate();
            }
            transform.position = originalPosition;
            t = 0;
            while (t <= .2f)
            {
                t += step;
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 0), t * 5f);
                yield return new WaitForFixedUpdate();
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
            Attacking = false;
        }

        StartCoroutine(StartAttacking());
    }
}

[System.Serializable]
public class EnemyStatus
{
    public int HealthPoints;
    public int Strength;
}
