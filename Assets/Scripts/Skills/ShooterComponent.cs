using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterComponent : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fireRate = 0.25f;

    [Header("Prefab")]
    [SerializeField] private Projectile projectilePrefab;

    private ObjectPooler<Projectile> projectilePool;

    private float elapsedTime = 0;

    private void Start()
    {
        projectilePool = new ObjectPooler<Projectile>(new GameObject("ProjectilePool").transform, projectilePrefab, 20);
    }

    //I'm not a huge fan of the Update function but in this case it doesn't do any harm
    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void Shoot(Vector3 targetPosition, float force)
    {
        Vector3 direction = (targetPosition - spawnPoint.position).normalized;

        Debug.DrawLine(spawnPoint.transform.position, spawnPoint.transform.position + (direction * 5f), Color.green);

        //The timer is always increasing, so every time the player shoots we reset the timer
        if (elapsedTime >= fireRate)
        {
            Projectile projectile = projectilePool.GetObject();
            projectile.gameObject.SetActive(true);

            projectile.transform.position = spawnPoint.transform.position;
            projectile.Shoot(direction, force);

            elapsedTime = 0;
        }

    }


}
