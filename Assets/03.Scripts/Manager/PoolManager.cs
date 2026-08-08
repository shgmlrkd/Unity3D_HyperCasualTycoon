using System.Collections.Generic;
using UnityEngine;

public class PoolManager : LocalSingleton<PoolManager>
{
    private readonly Dictionary<PoolType, Queue<Component>> poolDictionary = new Dictionary<PoolType, Queue<Component>>();

    private readonly Dictionary<PoolType, Transform> parentDictionary = new Dictionary<PoolType, Transform>();

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

            obj.gameObject.SetActive(false);

            pool.Enqueue(obj);
        }

        poolDictionary.Add(type, pool);
        parentDictionary.Add(type, parent);
    }

    public T Pop<T>(PoolType type) where T : Component
    {
        if (!poolDictionary.TryGetValue(type, out Queue<Component> pool))
        {
            return null;
        }

        if (pool.Count == 0)
            return null;

        T obj = pool.Dequeue() as T;

        if (obj != null)
            obj.gameObject.SetActive(true);

        return obj;
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