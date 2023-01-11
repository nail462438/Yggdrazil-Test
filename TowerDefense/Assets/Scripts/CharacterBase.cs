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

    [Header("Status Character")]
    public float damage;
    public float fireRate;

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
        stateCharacter = StateCharacter.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (stateCharacter)
        {
            case StateCharacter.Idle:
                {
                    switch (baseType)
                    {
                        case BaseType.TypeA:
                            {
                                if (view.CheckEnemyInRadius() != null)
                                {
                                    view.targetClosest = DesiredEnemy(view.CheckEnemyInRadius(), transform);
                                    stateCharacter = StateCharacter.Assault;
                                }
                                else Debug.Log("Idle");
                                break;
                            }
                        case BaseType.TypeB:
                            {
                                if (view.CheckEnemyInRadius() != null)
                                {
                                    view.targetClosest = DesiredEnemy(view.CheckEnemyInRadius(), transform);
                                    stateCharacter = StateCharacter.Assault;
                                }
                                else Debug.Log("Idle");
                                break;
                            }
                        case BaseType.TypeC:
                            {
                                if (view.CheckEnemyInRadius() != null)
                                {
                                    view.targetClosest = DesiredEnemy(view.CheckEnemyInRadius(), transform);
                                    stateCharacter = StateCharacter.Assault;
                                }
                                else Debug.Log("Idle");
                                break;
                            }
                    }
                    break;
                }
            case StateCharacter.Assault:
                {
                    if (view.CheckEnemyInRadius() != null)
                    {
                        if (view.targetClosest != null)
                        {
                            Debug.Log(view.targetClosest.gameObject.name);
                            if (baseType == view.targetClosest.GetComponent<TypeManager>().baseType)
                            {
                                // Damage * 0.5f
                            }
                            else
                            {
                                // Nomal Damage
                            }
                            currentTime = fireRate;
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

    private Transform DesiredEnemy(List<Transform> enemies, Transform fromTo)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromTo.position;
        foreach (Transform potentialTarget in enemies)
        {
            if (baseType != BaseType.TypeB)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
            else
            {

            }
        }
        return bestTarget;
    }
}
