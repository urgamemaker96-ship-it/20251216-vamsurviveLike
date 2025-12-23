using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Enemy, // 종류 구분
    Enemy0,
    Enemy1,
    Enemy2,
    Enemy3,
    Enemy4,

    Melee, // 종류 구분
    Melee0, // 삽
    Melee1, // 삼지창
    Melee2, // 낫

    Range, // 종류 구분
    Range0, // 엽총 (bullet3)
    Range1, // 기관총 (bullet4)
    Range2, // 샷건 (bullet5)

    Glove, // 종류 구분
    Glove0,

    Shoe, // 종류 구분
    Shoe0,

    Heal, // 종류 구분
    Heal0,
}

public class PoolManager : MonoBehaviour
{
    // Object Manage Pool
    private Dictionary<ObjectType, GameObject[]> objectPools;
    
    // Prefab
    [Header("# Object Prefab")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject[] bulletPrefabs;

    // Parent Transform
    [Header("# Parent Transform")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform bulletParent;

    private GameObject[] targetPool;

    private void Awake()
    {
        objectPools = new Dictionary<ObjectType, GameObject[]>();

        Generate();
    }

    private void Generate()
    {
        // Enemy
        objectPools[ObjectType.Enemy] = CreatePool(enemyPrefab, 300, enemyParent);

        // Bullet
        objectPools[ObjectType.Melee0] = CreatePool(bulletPrefabs[0], 10, bulletParent); // 삽 (Bullet 0)
        objectPools[ObjectType.Range0] = CreatePool(bulletPrefabs[1], 100, bulletParent);
    }

    private GameObject[] CreatePool(GameObject prefab, int size, Transform parent = null)
    {
        GameObject[] pool = new GameObject[size];

        for (int index = 0; index < pool.Length; index++)
        {
            pool[index] = Instantiate<GameObject>(prefab, parent);
            pool[index].SetActive(false);
        }

        return pool;
    }

    public GameObject GetObject(ObjectType objType)
    {
        targetPool = objectPools[objType];

        foreach (GameObject obj in targetPool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // 모든 Object가 활성화되어 있으면 null
        return null;
    }

    public int MaxObjectNum(ObjectType objectType)
    {
        return objectPools[objectType].Length;
    }
}
