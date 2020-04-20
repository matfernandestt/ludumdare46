using System;
using System.Collections;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private Transform[] EnemyPathPoints;

    private Coroutine currentCoroutine;
    private WaitForSeconds waitTime = new WaitForSeconds(2f);
    
    [Space, Header("Filled in runtime")]
    public Transform CurrentPoint;
    
    private void Awake()
    {
        GameEvents.OnPrepareToEnterBattle += StopAlternatingPoints;
        
        transform.parent = null;

        StartAlternatringPoints();
    }

    private void OnDestroy()
    {
        GameEvents.OnPrepareToEnterBattle -= StopAlternatingPoints;
    }
    
    public void StartAlternatringPoints()
    {
        if(currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(AlternatePathPoint());
    }
    
    public void StopAlternatingPoints()
    {
        if(currentCoroutine != null)
            StopCoroutine(currentCoroutine);
    }

    private IEnumerator AlternatePathPoint()
    {
        foreach (var point in EnemyPathPoints)
        {
            yield return waitTime;
            CurrentPoint = point;
        }
        if(currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(AlternatePathPoint());
    }

    private void OnDrawGizmos()
    {
        var previousPoint = EnemyPathPoints[0];
        var c = Color.red;
        foreach (var point in EnemyPathPoints)
        {
            Debug.DrawLine(previousPoint.position, point.position, c);
            previousPoint = point;
        }
        Debug.DrawLine(EnemyPathPoints[0].position, EnemyPathPoints[EnemyPathPoints.Length - 1].position, c);
        Gizmos.color = c;
        Gizmos.DrawWireSphere(CurrentPoint.position, .5f);
    }
}
