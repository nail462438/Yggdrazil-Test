using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateCharacter
{
    Idle,
    Assault,
    Prepare
}

[RequireComponent(typeof (TypeManager), typeof (FieldOfView))]
public class CharacterBase : MonoBehaviour
{
    public BaseType baseType { get; private set; }
    public StateCharacter stateCharacter { get; private set; }

    public List<GameObject> seenEnemies;
    public float coolTime;

    private FieldOfView view;
    private TypeManager typeManager;
    private float currentTime;

    private void Awake()
    {
        view = GetComponent<FieldOfView>();
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
        switch (stateCharacter)
        {
            case StateCharacter.Idle:
                {
                    if (view.CheckEnemyInRadius() != null)
                    {
                        view.targetClosest = GetClosestEnemy(view.CheckEnemyInRadius(), transform);
                        stateCharacter = StateCharacter.Assault;
                    }
                    else Debug.Log("Idle");
                    break;
                }
            case StateCharacter.Assault:
                {
                    if (view.CheckEnemyInRadius() != null)
                    {
                        if (view.targetClosest != null)
                        {
                            Debug.Log(view.targetClosest.gameObject.name);
                            currentTime = coolTime;
                            stateCharacter = StateCharacter.Prepare;
                        }
                        else stateCharacter = StateCharacter.Idle;
                    }
                    else stateCharacter = StateCharacter.Idle;
                    break;
                }
            case StateCharacter.Prepare:
                {
                    if (currentTime > 0) currentTime -= Time.deltaTime;
                    else stateCharacter = StateCharacter.Idle;
                    break;
                }
        }
    }

    private Transform GetClosestEnemy(List<Transform> enemies, Transform fromTo)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromTo.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
}
