using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateSpawner
{
    Start,
    Spawn,
    Stop,
    Break
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return ins; } }
    private static GameManager ins;

    public StateSpawner stateSpawner { get; private set; }

    [Header("Properties")]
    public List<Transform> pathManager;
    public GameObject[] enemies;
    public float startTime;
    public float coolTime;
    public float breakTime;
    public int maxEnemy;

    private int currentWave = 1;
    private int randomIndex;
    private int currentEnemy;
    private float currentTime;
    private float multipleStatus = 1f;

    private void Awake()
    {
        ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentEnemy = maxEnemy;
        currentTime = startTime;
        randomIndex = Random.Range(0, enemies.Length);
        UIManager.Instance.SetWaveText($"Wave : {currentWave}", Color.white);
        stateSpawner = StateSpawner.Start;
    }

    // Update is called once per frame
    void Update()
    {
        switch (stateSpawner)
        {
            case StateSpawner.Start:
                {
                    if (currentTime > 0) currentTime -= Time.deltaTime;
                    else
                    {
                        UIManager.Instance.SetActiveText(false);
                        stateSpawner = StateSpawner.Spawn;
                    }

                    UIManager.Instance.SetTimeText($"Prepare : {currentTime.ToString("F0")}", Color.white);
                    break;
                }
            case StateSpawner.Spawn:
                {
                    Debug.Log("Spawn");
                    if (currentEnemy > 0)
                    {
                        var obj = Instantiate(enemies[randomIndex], transform.position, Quaternion.Euler(0, 90, 0));
                        obj.GetComponent<EnemyBase>().hp *= multipleStatus;
                        obj.GetComponent<EnemyBase>().speed *= multipleStatus;
                        currentTime = coolTime;
                        if (randomIndex == 0) currentTime = coolTime;
                        else if (randomIndex == 1) currentTime = coolTime + 0.25f;
                        else if (randomIndex == 2) currentTime = coolTime + 0.75f;
                        stateSpawner = StateSpawner.Stop;
                    }
                    else
                    {
                        currentTime = breakTime;
                        UIManager.Instance.SetActiveText(true);
                        UIManager.Instance.SetTimeText($"Next Wave : {currentTime.ToString("F0")}", Color.red);
                        stateSpawner = StateSpawner.Break;
                    }
                    break;
                }
            case StateSpawner.Stop:
                {
                    Debug.Log("Stop");
                    if (currentTime > 0) currentTime -= Time.deltaTime;
                    else
                    {
                        currentEnemy--;
                        stateSpawner = StateSpawner.Spawn;
                    }
                    break;
                }
            case StateSpawner.Break:
                {
                    Debug.Log("Break");
                    UIManager.Instance.SetTimeText($"Next Wave : {currentTime.ToString("F0")}", Color.red);
                    if (currentTime > 0) currentTime -= Time.deltaTime;
                    else
                    {
                        currentEnemy = maxEnemy;
                        randomIndex = Random.Range(0, enemies.Length);
                        currentTime = coolTime;
                        currentWave++;
                        multipleStatus += 0.005f;
                        UIManager.Instance.SetWaveText($"Wave : {currentWave}", Color.white);
                        UIManager.Instance.SetActiveText(false);
                        stateSpawner = StateSpawner.Spawn;
                    }
                    break;
                }
        }
    }
}
