using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyBehaviour
{
    Scouting,
    Chasing
}

[SelectionBase]
public class BaseMovementAI : MonoBehaviour
{
    [HideInInspector] public int EnemyId;
    
    public CharacterController controller;
    [SerializeField] private EnemyPath pathToFollow;
    [SerializeField] private GameObject NoticedPlayerEffect;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float chasingSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private EnemyBehaviour CurrentEnemyBehaviour = EnemyBehaviour.Scouting;
    [SerializeField] private float distanceToBreakChase;
    

    private float hSpeed;
    private float zSpeed;
    private float vSpeed;
    [SerializeField] private bool cancelInputs;
    private Transform targetToChase;

    public EnemyBehaviour CurrentBehaviour => CurrentEnemyBehaviour;

    private void Awake()
    {
        GameEvents.OnPrepareToEnterBattle += StopMovement;
    }

    private void OnDestroy()
    {
        GameEvents.OnPrepareToEnterBattle -= StopMovement;
    }

    public void InstantDeath()
    {
        pathToFollow.StopAlternatingPoints();
        Destroy(pathToFollow.gameObject);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (cancelInputs) return;
        Movement();
        ApplyMovement();
        ChaseChain();
    }

    protected void Movement()
    {
        var dir = Vector3.zero;
        switch (CurrentEnemyBehaviour)
        {
            case EnemyBehaviour.Scouting:
                dir = pathToFollow.CurrentPoint.position - transform.position;
                break;
            case EnemyBehaviour.Chasing:
                dir = targetToChase.position - transform.position;
                break;
        }
        hSpeed = dir.x;
        zSpeed = dir.z;
        CheckRotation();
        switch (CurrentEnemyBehaviour)
        {
            case EnemyBehaviour.Scouting:
                hSpeed *= movementSpeed;
                zSpeed *= movementSpeed;
                break;
            case EnemyBehaviour.Chasing:
                hSpeed *= movementSpeed + chasingSpeed;
                zSpeed *= movementSpeed + chasingSpeed;
                break;
        }
    }

    protected void CheckRotation()
    {
        if (hSpeed > 0)
            transform.eulerAngles = transform.TransformDirection(new Vector3(0, 0, 0));
        else if (hSpeed < 0)
            transform.eulerAngles = transform.TransformDirection(new Vector3(0, 180, 0));
    }

    protected void ApplyMovement()
    {
        controller.Move(new Vector3(hSpeed, vSpeed, zSpeed) * Time.deltaTime / 3f);
        if (!controller.isGrounded)
            vSpeed -= gravity * Time.deltaTime;
    }

    protected void ChaseChain()
    {
        if (CurrentEnemyBehaviour == EnemyBehaviour.Chasing)
        {
            var distance = Vector3.Distance(transform.position, targetToChase.position);
            if (distance > distanceToBreakChase)
            {
                pathToFollow.StartAlternatringPoints();
                CurrentEnemyBehaviour = EnemyBehaviour.Scouting;
                targetToChase = null;
                NoticedPlayerEffect.SetActive(false);
            }
        }
    }

    protected void StopMovement()
    {
        cancelInputs = true;
    }

    public void StartChase(Transform target)
    {
        targetToChase = target;
        CurrentEnemyBehaviour = EnemyBehaviour.Chasing;
        pathToFollow.StopAlternatingPoints();
        NoticedPlayerEffect.SetActive(true);
    }

    public void Jump()
    {
        if (controller.isGrounded)
            vSpeed = jumpSpeed;
    }

    public static bool RandomBool()
    {
        int v = Random.Range(0, 2);
        return (v != 0);
    }
}
