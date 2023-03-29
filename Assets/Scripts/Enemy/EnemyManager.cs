using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    [Header("Settings")]
    [SerializeField] private int maxEnemies = 30;

    [Header("Prefabs")]
    [SerializeField] private Enemy enemyPrefab;

    private ObjectPooler<Enemy> enemyPool;
    private List<Enemy> activeEnemies = new List<Enemy>();

    private PlayerHealth playerReference;
    public PlayerHealth PlayerReference => playerReference ??= FindObjectOfType<PlayerHealth>();

    private void Awake()
    {
        Instance = this;
        enemyPool = new ObjectPooler<Enemy>(transform, enemyPrefab, maxEnemies);
    }

    public Enemy GetEnemy()
    {
        if (activeEnemies.Count >= maxEnemies) return null;

        Enemy enemy = enemyPool.GetObject();
        enemy.AssignPlayerReference(PlayerReference);
        enemy.gameObject.SetActive(true);

        AddToActiveEnemies(enemy);
        return enemy;
    }

    private void AddToActiveEnemies(Enemy enemyToAdd)
    {
        if (!activeEnemies.Contains(enemyToAdd))
        {
            activeEnemies.Add(enemyToAdd);
        }
    }

    public void RemoveFromActiveEnemies(Enemy enemyToRemove)
    {
        if (activeEnemies.Contains(enemyToRemove))
        {
            activeEnemies.Remove(enemyToRemove);
        }
    }
}
