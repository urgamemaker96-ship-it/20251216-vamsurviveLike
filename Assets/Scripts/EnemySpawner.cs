using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnData[] spawnData;

    private Transform[] spawnPoints;
    private Coroutine enemySpawnCoroutine;
    private float spawnInterval;

    private float levelTime;
    private int level; // 현재 레벨

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        levelTime = GameManager.Instance.MaxGameTime / spawnData.Length;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameStop)
        {
            return;
        }
        
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime / levelTime), spawnData.Length - 1);
    }

    private void Start()
    {
        StartEnemySpawn();
    }

    private void StartEnemySpawn()
    {
        enemySpawnCoroutine = StartCoroutine(EnemySpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // Level에 따라 SpawnInterval 감소
        }
    }

    private void SpawnEnemy()
    {
        int spawnIndex = Random.Range(1, spawnPoints.Length); // EnemySpawner 자체의 Position은 제외
        GameObject enemy = GameManager.Instance.PoolManager.GetObject(ObjectType.Enemy); // Object Pool에서 하나 받아옴

        enemy.GetComponent<Enemy>().Init(spawnData[level]); // 현재 게임 진행도에 따라 Enemy 종류 설정
        enemy.transform.position = spawnPoints[spawnIndex].position;
        spawnInterval = spawnData[level].SpawnInterval;
    }

    public void StopEnemySpawn()
    {
        StopCoroutine(enemySpawnCoroutine);
    }
}

[System.Serializable]
public class SpawnData
{
    [SerializeField] private float spawnInterval;
    public float SpawnInterval => spawnInterval;
    [SerializeField] private ObjectType objType;
    public ObjectType ObjType => objType;
    [SerializeField] private RuntimeAnimatorController aniController;
    public RuntimeAnimatorController AniController => aniController;
    [SerializeField] private float hp;
    public float Hp => hp;
    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;
}
