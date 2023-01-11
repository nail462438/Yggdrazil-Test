using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public Transform targetClosest;

    public LayerMask targetMask;

    public bool canSeePlayer;

    private void Start()
    {
        //targetClosest = GameObject.FindGameObjectWithTag("Player");
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
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius + 0.5f, targetMask);

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

    public List<Transform> CheckEnemyInRadius()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius + 0.5f, targetMask);
        List<Transform> enemies = new List<Transform>();

        if (rangeChecks.Length != 0) 
        {
            foreach (var obj in rangeChecks)
            {
                enemies.Add(obj.transform);
            }
        }

        return enemies;
    }
}