using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(TypeManager))]
public class EnemyBase : MonoBehaviour
{
    public BaseType baseType { get; private set; }

    [Header("Status Enemy")]
    public float hp;
    public float speed;

    [Space(5)]
    [Header("Auto Pathway")]
    public List<Transform> pathWay;

    private TypeManager typeManager;

    private int currentPath = 0;
    private int truePath = 0;
    private bool checkPoint = false;

    private void Awake()
    {
        typeManager = GetComponent<TypeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        baseType = typeManager.baseType;
        pathWay = GameManager.Instance.pathManager;
    }

    private void Update()
    {
        var targetRot = Quaternion.LookRotation(pathWay[truePath].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 6 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, pathWay[truePath].position, (speed / 4f) * Time.deltaTime);

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
            Destroy(gameObject);
        }
    }
}
