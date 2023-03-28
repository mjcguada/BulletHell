using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    [Header("Prefabs")]
    [SerializeField] private Enemy enemyPrefab;

    private ObjectPooler<Enemy> enemyPool;

    private PlayerHealth playerReference;
    public PlayerHealth PlayerReference => playerReference ??= FindObjectOfType<PlayerHealth>();

    private void Awake()
    {
        Instance = this;
        enemyPool = new ObjectPooler<Enemy>(transform, enemyPrefab, 20);
    }

    public Enemy GetEnemy()
    {
        Enemy enemy = enemyPool.GetObject();
        enemy.AssignPlayerReference(PlayerReference);
        enemy.gameObject.SetActive(true);
        return enemy;
    }
}
