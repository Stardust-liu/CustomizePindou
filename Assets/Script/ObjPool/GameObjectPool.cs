using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : BaseInstance<GameObjectPool>
{
    public enum Pool
    {
        Pixel,
    }
    public Dictionary<Pool, List<GameObject>> poolDictionary;
    public GameObjectPool()
    {
        if (poolDictionary == null)
            poolDictionary = new Dictionary<Pool, List<GameObject>>();
    }

    /// <summary>
    /// 获取对象
    /// </summary>
    /// <param name="pool"></param>
    /// <returns></returns>
    public GameObject GetPool(Pool pool)
    {
        GameObject obj = null;
        if (!poolDictionary.ContainsKey(pool))
        {
            Debug.Log("没有对象");
            return obj;
        }
        else
        {
            foreach (var item in poolDictionary[pool])
            {
                if (!item.activeInHierarchy)
                    obj = item;
            }
            if (obj == null)
            {
                GameObject careatItem = GameObject.Instantiate(poolDictionary[pool][0]);
                obj = careatItem;
                AddPool(pool, careatItem);
            }
        }
        return obj;
    }

    /// <summary>
    /// 添加对象
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="gameObject"></param>
    public void AddPool(Pool pool,GameObject gameObject)
    {
        if (!poolDictionary.ContainsKey(pool))
        {
            var list = new List<GameObject>();
            list.Add(gameObject);
            poolDictionary.Add(pool, list);
        }
        else
        {
            poolDictionary[pool].Add(gameObject);
        }
    }
    /// <summary>
    /// 隐藏所有池中对象
    /// </summary>
    /// <param name="pool"></param>
    public void HidePool(Pool pool)
    {
        if (poolDictionary.ContainsKey(pool))
        {
            foreach (var item in poolDictionary[pool])
            {
                item.SetActive(false);
            }
        }
    }
}
