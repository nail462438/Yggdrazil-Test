using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TypeManager))]
public class EnemyBase : MonoBehaviour
{
    public BaseType baseType { get; private set; }

    [Header("Status Enemy")]
    public int hp;
    public float speed;

    private TypeManager typeManager;

    private void Awake()
    {
        typeManager = GetComponent<TypeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        baseType = typeManager.baseType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
