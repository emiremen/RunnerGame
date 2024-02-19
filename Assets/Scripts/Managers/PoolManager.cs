using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;



    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int poolSize;
    }

    void OnEnable()
    {
        EventManager.callObjectFromPool += CallObjectFromPool;
    }
    void OnDisable()
    {
        EventManager.callObjectFromPool -= CallObjectFromPool;
    }

    void Awake()
    {
        poolDictionary = new();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject spawnedObject = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
                
                spawnedObject.SetActive(false);
                objectPool.Enqueue(spawnedObject);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }


    }

    private GameObject CallObjectFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with {tag} tag doesn't exist!");
            return null;
        }
        GameObject calledObject = poolDictionary[tag].Dequeue();
        calledObject.SetActive(true);

        poolDictionary[tag].Enqueue(calledObject);
        return calledObject;
    }
}
