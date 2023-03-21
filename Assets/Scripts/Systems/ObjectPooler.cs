using UnityEngine;

public class ObjectPooler<T> : MonoBehaviour where T : MonoBehaviour
{
    private T objectToPool;
    private int poolSize = 30;

    private T[] poolObject;

    public ObjectPooler(T prefab, int poolSize)
    {
        objectToPool = prefab;
        this.poolSize = poolSize;

        if (poolSize <= 0) return;        

        poolObject = new T[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            poolObject[i] = Instantiate(objectToPool);
            poolObject[i].gameObject.SetActive(false);
        }
    }

    public T GetObject()
    {
        for (int i = 0; i < poolObject.Length; i++)
        {
            if (!poolObject[i].gameObject.activeInHierarchy)
            {
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

        T[] newPool = new T[poolSize];

        //We make a copy of the old pool
        for (int i = 0; i < poolObject.Length; i++)
        {
            newPool[i] = poolObject[i];
        }

        for (int i = oldPoolSize; i < poolSize; i++)
        {
            newPool[i] = Instantiate(objectToPool);
            newPool[i].gameObject.SetActive(false);
        }

        //The new pool becomes the current pool but with double size
        poolObject = newPool;
    }
}
