using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : Singleton<ObjectPool<T>> where T : MonoBehaviour  // fixed extra >, fixed inheritance
{
    [SerializeField] protected T prefab;
    private List<T> pooledObjects;  // added type parameter
    private int amount;
    private bool isReady;

    // create the pool
    public void PoolObjects(int amount = 0)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException("Amount to pool must be non-negative");
        this.amount = amount;
        pooledObjects = new List<T>(amount);  // removed duplicate initialization

        // instantiate T
        GameObject newObject;
        for (int i = 0; i != amount; ++i)  // fixed ++1 -> ++i
        {
            newObject = Instantiate(prefab.gameObject, transform);
            newObject.SetActive(false);
            // add each T to the list
            pooledObjects.Add(newObject.GetComponent<T>());
        }
        // flag the pool ready
        isReady = true;
    }

    // get an object from the pool
    public T GetPooledObject()
    {
        // check if pool ready
        if (!isReady)
            throw new InvalidOperationException("Pool is not ready. Call PoolObjects() first.");  // fixed stray expression

        // search through list
        for (int i = 0; i != amount; ++i)  // fixed extra semicolon
            if (!pooledObjects[i].isActiveAndEnabled)
                return pooledObjects[i];

        // if we don't find one, make a new one
        GameObject newObject = Instantiate(prefab.gameObject, transform);
        newObject.SetActive(false);
        pooledObjects.Add(newObject.GetComponent<T>());  // added ()
        ++amount;
        return newObject.GetComponent<T>();
    }

    // return object back to pool
    public void ReturnObjectToPool(T toBeReturned)  // unified parameter name
    {
        // verify the argument
        if (toBeReturned == null)
            return;

        // make sure pool is ready
        if (!isReady)
        {
            PoolObjects(1);  // pool at least 1 so isReady gets set
            pooledObjects.Add(toBeReturned);
            ++amount;
        }

        // deactivate game object
        toBeReturned.gameObject.SetActive(false);
    }
}