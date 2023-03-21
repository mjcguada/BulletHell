using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerReference;
    [SerializeField] private ObjectPooler enemyPool;

    public static EnemyManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetEnemy() 
    {
        return enemyPool.GetObject();
    }
}
