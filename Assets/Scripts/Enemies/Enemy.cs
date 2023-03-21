using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private PlayerHealth playerReference;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        StartCoroutine(FollowPlayerCoroutine());
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


    private void OnDisable()
    {
        StopAllCoroutines(); //TODO: Revisar
        EnemyManager.Instance.RemoveFromActiveEnemies(this);
    }

}
