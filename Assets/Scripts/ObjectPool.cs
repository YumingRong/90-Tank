using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Dictionary<string, GameObject> prefabs;
    private Dictionary<string, Stack<GameObject>> pool;
    private static ObjectPool instance;

    public static ObjectPool GetInstance()
    {
        if (instance == null)
        {
            instance = new ObjectPool();
        }
        return instance;
    }

    private ObjectPool()
    {
        pool = new Dictionary<string, Stack<GameObject>>();
        prefabs = new Dictionary<string, GameObject>();
    }

    public GameObject GetObject(string objName)
    {
        GameObject result = null;

        if (pool.ContainsKey(objName))
        {
            if (pool[objName].Count > 0)
            {
                result = pool[objName].Pop();
                result.SetActive(true);
                return result;
            }
        }

        GameObject prefab = null;
        if (prefabs.ContainsKey(objName))
        {
            prefab = prefabs[objName];
        }
        else
        {
            prefab = Resources.Load<GameObject>("Prefabs/" + objName);
            prefabs.Add(objName, prefab);
        }
        result = Object.Instantiate(prefab);
        //改名（去除clone）
        result.name = objName;
        return result;
    }

    public void RecycleObj(GameObject obj)
    {
        obj.SetActive(false);
        if (pool.ContainsKey(obj.name))
        {
            pool[obj.name].Push(obj);
        }
        else
        {
            Stack<GameObject> stack = new Stack<GameObject>();
            pool.Add(obj.name, stack);
            stack.Push(obj);
        }
    }

    public void Clear()
    {
        foreach (string obj in pool.Keys)
        {
            pool[obj].Clear();
        }
    }
}
