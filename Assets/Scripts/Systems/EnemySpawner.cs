using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Color spawnerColor = Color.green;
    [SerializeField] private float frequenceInSeconds = 1.0f;

    private BoxCollider collider = null;

    private void Awake()
    {
        collider = gameObject.GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    private IEnumerator SpawnEnemiesCoroutine() 
    {
        while (gameObject.activeSelf) 
        {
            Vector3 enemyPosition = Vector3.Lerp(collider.bounds.min, collider.bounds.max, Random.Range(0.0f, 1.0f));
            GameObject newEnemy = EnemyManager.Instance.GetEnemy();
            newEnemy.transform.position = enemyPosition;

            yield return new WaitForSeconds(frequenceInSeconds);        
        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = spawnerColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
#endif

}
