using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Stack<GameObject> pool;
    private static ObjectPool instance;

    public static ObjectPool GetInstance()
    {
        if (instance == null)
        {
            instance = new ObjectPool();
        }
        return instance;
    }

    public void Awake(string objName)
    {
        if (prefab == null)
            prefab = Resources.Load<GameObject>("Prefabs/" + objName);
    }

    public ObjectPool()
    {
        pool = new Stack<GameObject>();
    }

    public GameObject GetObject(Vector3 position, Quaternion quaternion)
    {
        GameObject result;
        if (pool.Count>0)
        {
            result = pool.Pop();
            result.transform.position = position;
            result.transform.rotation = quaternion;
            result.SetActive(true);
        }
        else
        {
            result = Object.Instantiate(prefab);
            result.transform.position = position;
            result.transform.rotation = quaternion;
            result.name = prefab.name;
        }
        return result;
    }

    public void RecycleObj(GameObject obj)
    {
        obj.SetActive(false);
        pool.Push(obj);
    }
}
