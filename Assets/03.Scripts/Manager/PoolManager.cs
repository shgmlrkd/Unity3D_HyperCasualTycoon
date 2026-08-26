using System.Collections.Generic;
using UnityEngine;

public class PoolManager : LocalSingleton<PoolManager>
{
    private readonly Dictionary<PoolType, Queue<Component>> poolDictionary = new Dictionary<PoolType, Queue<Component>>();

    private readonly Dictionary<PoolType, Transform> parentDictionary = new Dictionary<PoolType, Transform>();
    
    // Pool 확장 시 사용할 원본 Prefab
    private readonly Dictionary<PoolType, Component> prefabDictionary = new Dictionary<PoolType, Component>();

    // 고유 ID가 필요한 오브젝트라면 Pool이 부족할 때 생성되는 오브젝트에 이어서 고유 ID를 붙히기위한 Dictionary
    private readonly Dictionary<PoolType, int> poolCountDictionary = new Dictionary<PoolType, int>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void CreatePool<T>(PoolType type, T prefab, int count) where T : Component
    {
        if (poolDictionary.ContainsKey(type))
            return;

        GameObject parentObject = new GameObject(type.ToString());
        parentObject.transform.SetParent(transform);

        Transform parent = parentObject.transform;

        Queue<Component> pool = new Queue<Component>();

        for (int i = 0; i < count; i++)
        {
            T obj = Instantiate(prefab, parent);

            if (obj is IPoolInitialize poolInitialize)
            {
                poolInitialize.InitializePool(i);
            }

            obj.gameObject.SetActive(false);

            pool.Enqueue(obj);
        }

        poolDictionary.Add(type, pool);
        parentDictionary.Add(type, parent);
        prefabDictionary.Add(type, prefab);
        poolCountDictionary.Add(type, count);
    }

    public T Pop<T>(PoolType type) where T : Component
    {
        if (!poolDictionary.TryGetValue(type, out Queue<Component> pool))
        {
            return null;
        }

        if (pool.Count == 0)
        {
            if (!prefabDictionary.TryGetValue(type, out Component prefab))
                return null;

            if (!parentDictionary.TryGetValue(type, out Transform parent))
                return null;

            T obj = Instantiate(prefab, parent) as T;
            
            if (obj == null)
                return null;

            if (obj is IPoolInitialize poolInitialize)
            {
                int ID = poolCountDictionary[type]++;
                poolInitialize.InitializePool(ID);
            }

            obj.gameObject.SetActive(true);

            return obj;
        }


        T poolObj = pool.Dequeue() as T;

        if (poolObj != null)
            poolObj.gameObject.SetActive(true);

        return poolObj;
    }

    public void Release<T>(PoolType type, T obj) where T : Component
    {
        if (obj == null)
            return;

        if (!poolDictionary.TryGetValue(type, out Queue<Component> pool))
        {
            return;
        }

        obj.gameObject.SetActive(false);

        pool.Enqueue(obj);
    }
}