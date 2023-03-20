using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int poolSize = 30;

    [SerializeField] private RuntimeSet<GameObject> enemyRuntimeSet;

    private GameObject[] poolObject;

    private void Start()
    {
        if (objectToPool == null || poolSize <= 0)
        {
            Destroy(this);
            Destroy(gameObject);
            return;
        }

        enemyRuntimeSet.Initialize(); //Important to clear existing list

        poolObject = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            poolObject[i] = Instantiate(objectToPool);
            poolObject[i].SetActive(false);
            enemyRuntimeSet.AddToList(poolObject[i]);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < poolObject.Length; i++)
        {
            if (!poolObject[i].activeInHierarchy)
            {
                poolObject[i].SetActive(true);
                return poolObject[i];
            }
        }

        //This part of the function is called if there's no object available
        ExpandPoolSize();

        return GetObject();
    }

    private void ExpandPoolSize()
    {
        int oldPoolSize = poolSize;
        poolSize = poolSize * 2;

        GameObject[] newPool = new GameObject[poolSize];

        //We make a copy of the old pool
        for (int i = 0; i < poolObject.Length; i++)
        {
            newPool[i] = poolObject[i];
        }

        for (int i = oldPoolSize; i < poolSize; i++)
        {
            newPool[i] = Instantiate(objectToPool);
            newPool[i].SetActive(false);
            enemyRuntimeSet.AddToList(newPool[i]);
        }

        poolObject = newPool;
    }
}
