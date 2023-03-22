using UnityEngine;

public class ObjectPooler<T> : MonoBehaviour where T : MonoBehaviour, IReseteable
{
    private T objectToPool;
    private int poolSize = 30;
    private T[] pool;

    private Transform parentTransform;

    public ObjectPooler(Transform parentTransform, T prefab, int poolSize)
    {
        objectToPool = prefab;
        this.parentTransform = parentTransform;
        this.poolSize = poolSize;

        if (poolSize <= 0) return;

        pool = new T[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Create();
        }
    }

    private T Create()
    {
        T newObject = Instantiate(objectToPool);
        newObject.transform.SetParent(parentTransform);
        newObject.gameObject.SetActive(false);

        return newObject;
    }

    public T GetObject()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                return pool[i];
            }
        }

        //This part of the function is called if there's no object available
        ExpandPoolSize();

        return GetObject();
    }

    public void ResetObjects()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i].Reset();
        }
    }

    private void ExpandPoolSize()
    {
        int oldPoolSize = poolSize;
        poolSize = poolSize * 2;

        T[] newPool = new T[poolSize];

        //We make a copy of the old pool
        for (int i = 0; i < pool.Length; i++)
        {
            newPool[i] = pool[i];
        }

        for (int i = oldPoolSize; i < poolSize; i++)
        {
            newPool[i] = Create();
        }

        //The new pool becomes the current pool but with double size
        pool = newPool;
    }
}
