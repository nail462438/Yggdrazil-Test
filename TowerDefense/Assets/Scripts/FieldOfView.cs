using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radiusShoot;
    [Range(0, 360)]
    public float angle;

    public Transform targetBest;

    public LayerMask targetMask;

    public bool canSeePlayer;

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radiusShoot + 0.5f, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                canSeePlayer = true;
            else 
                canSeePlayer = false;
        }
        else if (canSeePlayer) 
            canSeePlayer = false;
    }

    public List<GameObject> CheckEnemyInRadius()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radiusShoot + 0.5f, targetMask);
        List<GameObject> enemies = new List<GameObject>();

        if (rangeChecks.Length != 0)
        {
            foreach (var obj in rangeChecks)
            {
                enemies.Add(obj.gameObject);
            }
        }
        else if (canSeePlayer)
            canSeePlayer = false;

        return enemies;
    }
}