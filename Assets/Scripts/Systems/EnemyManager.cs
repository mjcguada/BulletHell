using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerReference;
    [SerializeField] private Enemy enemyPrefab;

    private ObjectPooler<Enemy> enemyPool;

    public static EnemyManager Instance;

    private List<Enemy> activeEnemies = new List<Enemy>();

    private void Awake()
    {
        Instance = this;
        enemyPool = new ObjectPooler<Enemy>(enemyPrefab, 20);
    }

    public Enemy GetEnemy()
    {
        Enemy enemy = enemyPool.GetObject();
        enemy.AssignPlayerReference(playerReference);
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

    public Enemy GetNearestEnemy(Vector3 position) 
    {
        float minDistance = float.MaxValue;
        Enemy nearestEnemy = null;

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            Enemy currentEnemy = activeEnemies[i];
            float distanceToPosition = Vector3.Distance(position, currentEnemy.transform.position);
            
            if (distanceToPosition < minDistance) 
            {
                nearestEnemy = currentEnemy;
                minDistance = distanceToPosition;
            }
        }

        return nearestEnemy;
    }
}
