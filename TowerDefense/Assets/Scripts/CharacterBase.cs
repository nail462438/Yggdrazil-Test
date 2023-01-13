using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Transform spawnPos;
    public GameObject ammoPrefab;
    public float speedAmmo;

    private FieldOfView view;
    private TypeManager typeManager;
    private float currentTime;
    private Animator anim;
    [HideInInspector] public int basePrice;
    [HideInInspector] public string nameTower;
    public List<GameObject> goalEnemyies;

    private void Awake()
    {
        view = GetComponent<FieldOfView>();
        typeManager = GetComponent<TypeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        baseType = typeManager.baseType;
        anim = GetComponentInChildren<Animator>();
        stateCharacter = StateCharacter.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("sell")) return;

        if (view.targetBest != null)
        {
            var targetRot = Quaternion.LookRotation(new Vector3(view.targetBest.position.x, 0, view.targetBest.position.z) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10 * Time.deltaTime);
        }

        switch (stateCharacter)
        {
            case StateCharacter.Idle:
                {
                    switch (baseType)
                    {
                        case BaseType.TypeA:
                            {
                                if (view.CheckEnemyInRadius().Count != 0)
                                {
                                    view.targetBest = ClosestEnemy(view.CheckEnemyInRadius(), transform);
                                    stateCharacter = StateCharacter.Assault;
                                }
                                //else Debug.Log("Idle");
                                break;
                            }
                        case BaseType.TypeB:
                            {
                                if (view.CheckEnemyInRadius().Count != 0)
                                {
                                    goalEnemyies = view.CheckEnemyInRadius();
                                    view.targetBest = FarthestAndMostHpEnemy(goalEnemyies);
                                    stateCharacter = StateCharacter.Assault;
                                }
                                //else Debug.Log("Idle");
                                break;
                            }
                        case BaseType.TypeC:
                            {
                                if (view.CheckEnemyInRadius().Count != 0)
                                {
                                    view.targetBest = ClosestEnemy(view.CheckEnemyInRadius(), transform);
                                    stateCharacter = StateCharacter.Assault;
                                }
                                //else Debug.Log("Idle");
                                break;
                            }
                    }
                    break;
                }
            case StateCharacter.Assault:
                {
                    if (view.CheckEnemyInRadius() != null)
                    {
                        if (view.targetBest != null)
                        {
                            anim.SetTrigger("attack");
                            if (baseType == view.targetBest.GetComponent<TypeManager>().baseType)
                            {
                                // Damage * 1.5f
                                var obj = Instantiate(ammoPrefab, spawnPos.position, Quaternion.identity);
                                obj.GetComponent<AmmoBase>().target = view.targetBest;
                                obj.GetComponent<AmmoBase>().damage = damage * 1.50f;
                                obj.GetComponent<AmmoBase>().speed = speedAmmo;

                                //view.targetBest.GetComponent<EnemyBase>().hp -= damage * 1.50f;
                            }
                            else
                            {
                                // Nomal Damage
                                var obj = Instantiate(ammoPrefab, spawnPos.position, Quaternion.identity);
                                obj.GetComponent<AmmoBase>().target = view.targetBest;
                                obj.GetComponent<AmmoBase>().damage = damage;
                                obj.GetComponent<AmmoBase>().speed = speedAmmo;

                                //view.targetBest.GetComponent<EnemyBase>().hp -= damage;
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

    private Transform ClosestEnemy(List<GameObject> enemies, Transform fromTo)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromTo.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }
        return bestTarget;
    }

    private Transform FarthestAndMostHpEnemy(List<GameObject> enemies)
    {
        Transform bestTarget = null;
        float mostHp = 0;
        int indexTarget = 0;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetComponent<EnemyBase>().hp > mostHp)
            {
                mostHp = enemies[i].GetComponent<EnemyBase>().hp;
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (mostHp == enemies[i].GetComponent<EnemyBase>().hp)
            {
                indexTarget = i;
                break;
            }
        }

        //Debug.Log($"Hp : {mostHp} || Distance : {distanceFar}");

        bestTarget = enemies[indexTarget].transform;

        return bestTarget;
    }

    private void UpgradeTower(float money)
    {
        if (money > basePrice * 5)
        {
            damage *= 1.5f;
            basePrice *= 2;
        }
        else
        {
            StartCoroutine(ClosePopupAuto());
        }
    }

    private void SellTower()
    {
        StartCoroutine(SellProcess());
    }

    IEnumerator SellProcess()
    {
        anim.SetBool("sell", true);
        UIManager.Instance.SetActiveText(UIManager.Instance.myCommand, false);
        GameManager.Instance.money += (basePrice / 2);
        yield return new WaitForSeconds(3);
        GetComponentInParent<BoxPlane>().busy = false;
        Destroy(gameObject);
    }

    IEnumerator ClosePopupAuto()
    {
        UIManager.Instance.SetActiveText(UIManager.Instance.popupText, true);
        UIManager.Instance.SetText(UIManager.Instance.popupText, $"Money not enought", Color.red);
        yield return new WaitForSeconds(2);
        UIManager.Instance.SetActiveText(UIManager.Instance.popupText, false);
    }

    private void OnMouseDown()
    {
        if (GetComponentInParent<BoxPlane>() == null) return;

        if (GetComponentInParent<BoxPlane>().busy)
        {
            UIManager.Instance.upgradeButton.GetComponent<Button>().onClick.RemoveAllListeners();
            UIManager.Instance.sellButton.GetComponent<Button>().onClick.RemoveAllListeners();

            UIManager.Instance.SetActiveText(UIManager.Instance.myCommand, true);
            UIManager.Instance.SetText(UIManager.Instance.nameTowerText, $"{nameTower}", Color.white);
            UIManager.Instance.upgradeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"UPGRADE\n{basePrice * 6}";
            UIManager.Instance.sellButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"SELL\n{basePrice / 2}";
            UIManager.Instance.upgradeButton.GetComponent<Button>().onClick.AddListener(() => UpgradeTower(GameManager.Instance.money));
            UIManager.Instance.sellButton.GetComponent<Button>().onClick.AddListener(() => SellTower());
        }
    }
}
