using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IReseteable
{
    [Tooltip("The point the player will be shooting")]
    [SerializeField] private Transform targetTransform;

    private NavMeshAgent navMeshAgent;
    private PlayerHealth playerReference;
    public Vector3 TargetPosition => targetTransform.position;

    //TODO: establish from the EnemyManager
    private int health = 1;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        StartCoroutine(FollowPlayerCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>() != null)
        {
            health--;
            if (health <= 0)
            {
                Reset();
            }
        }
    }

    private IEnumerator FollowPlayerCoroutine()
    {
        while (gameObject.activeSelf)
        {
            if (navMeshAgent != null && playerReference != null)
            {
                navMeshAgent.SetDestination(playerReference.transform.position);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void AssignPlayerReference(PlayerHealth playerReference)
    {
        this.playerReference = playerReference;
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines(); //TODO: Revisar
        EnemyManager.Instance.RemoveFromActiveEnemies(this);
    }
}
