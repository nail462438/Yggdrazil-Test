using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBase : MonoBehaviour
{
    public GameObject effect;
    [HideInInspector] public Transform target;
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;

    [Header("Debuff")]
    public bool slow;
    public bool bomb;

    private void Start()
    {
        StartCoroutine(ProcessDamage());
    }

    IEnumerator ProcessDamage()
    {
        WaitForSeconds wait = new WaitForSeconds(0.001f);

        while (true)
        {
            yield return wait;
            if (target == null) Destroy(gameObject);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.position) <= 0.1f)
            {
                break;
            }
        }

        Instantiate(effect, transform);
        if (slow && !target.GetComponent<EnemyBase>().isSlow) target.GetComponent<EnemyBase>().FrezeSlow();
        if (bomb) target.GetComponent<EnemyBase>().Bomb(damage, 1);
        target.GetComponent<EnemyBase>().hp -= damage;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
