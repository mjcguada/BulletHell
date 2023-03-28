using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IReseteable
{
    [SerializeField] private Material damageMaterial;

    private NavMeshAgent navMeshAgent;
    private PlayerHealth playerReference;

    //TODO: establish from the EnemyManager
    private int health = 4;

    private Material originalMaterial;
    private SkinnedMeshRenderer myMeshRenderer;
    private Coroutine blinkCoroutine;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        myMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originalMaterial = myMeshRenderer.material;
    }

    private void OnEnable()
    {
        StartCoroutine(FollowPlayerCoroutine());
    }

    public void AssignPlayerReference(PlayerHealth playerReference)
    {
        this.playerReference = playerReference;
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

    private void OnTriggerEnter(Collider other)
    {
        DamageDealer damage = other.gameObject.GetComponent<DamageDealer>();
        if (damage != null)
        {
            ReceiveDamage();            
        }
    }

    private void ReceiveDamage() 
    {
        if(blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(ChangeToDamagedMaterial());

        health--;
        if (health <= 0)
        {
            Reset();
        }
    }

    private IEnumerator ChangeToDamagedMaterial() 
    {
        myMeshRenderer.material = damageMaterial;
        yield return new WaitForSeconds(0.1f);
        myMeshRenderer.material = originalMaterial;
    }

    //IReseteable interface
    public void Reset()
    {
        myMeshRenderer.material = originalMaterial;
        gameObject.SetActive(false);
    }
}
