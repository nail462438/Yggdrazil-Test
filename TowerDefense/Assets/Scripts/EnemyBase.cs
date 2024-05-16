using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(TypeManager), typeof (FieldOfView))]
public class EnemyBase : MonoBehaviour
{
    public BaseType baseType { get; private set; }

    [Header("Status Enemy")]
    public string nameEnemy;
    public float hp;
    public float speed;
    public Slider hpBar;

    [Space(5)]
    [Header("Auto Pathway")]
    public List<Transform> pathWay;

    private TypeManager typeManager;
    private FieldOfView view;

    private int currentPath = 0;
    private int truePath = 0;
    private bool checkPoint = false;

    [Header("Resistant Enemy")]
    [HideInInspector]
    public bool isSlow = false;

    private void Awake()
    {
        view = GetComponent<FieldOfView>();
        typeManager = GetComponent<TypeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        baseType = typeManager.baseType;
        pathWay = GameManager.Instance.pathManager;
        hpBar.maxValue = hp;
    }

    private void Update()
    {
        float sp = isSlow ? speed * 0.65f : speed;

        var targetRot = Quaternion.LookRotation(pathWay[truePath].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 6 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, pathWay[truePath].position, (sp / 4f) * Time.deltaTime);
        hpBar.value = hp;

        var dis = Vector3.Distance(transform.position, pathWay[truePath].position);

        if (dis < 0.01f && !checkPoint)
        {
            currentPath++;
            if (currentPath < pathWay.Count) truePath = currentPath;
            checkPoint = true;
        }
        else if (dis > 0.01f && checkPoint)
        {
            checkPoint = false;
        }
        else if (dis <= 0.01f && truePath == pathWay.Count - 1)
        {
            GameManager.Instance.hp--;
            Destroy(gameObject);
        }

        if (hp <= 0)
        {
            switch (baseType)
            {
                case BaseType.TypeA:
                    GameManager.Instance.money += 20;
                    break;
                case BaseType.TypeB:
                    GameManager.Instance.money += 30;
                    break;
                case BaseType.TypeC:
                    GameManager.Instance.money += 40;
                    break;
            }

            GameManager.Instance.EnemiesInRound.Remove(this);
            Destroy(gameObject);
        }
    }

    public void Bomb(float damageBomb, float radius)
    {
        view.radiusShoot = radius;
        
        foreach (var enemies in view.CheckEnemyInRadius())
        {
            enemies.GetComponent<EnemyBase>().hp -= damageBomb;
        }
    }

    public void FrezeSlow()
    {
        StartCoroutine(SlowResist());
    }

    IEnumerator SlowResist()
    {
        isSlow = true;
        yield return new WaitForSeconds(3);
        isSlow = false;
    }
}
