using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    private readonly static Dictionary<GameObject, Pool> pools=new Dictionary<GameObject, Pool>();

    /// <summary>
    /// Linked List node associated with each GameObject Instances
    /// </summary>
    private readonly static Dictionary<GameObject, KeyValuePair<Pool,LinkedListNode<GameObject>>> nodes = new Dictionary<GameObject, KeyValuePair<Pool,LinkedListNode<GameObject>>>();

    public static int globalPoolSize = 10;

    /// <summary>
    /// Instantiate a gameobject into the pool transform and other stuff needs to be set via script
    /// </summary>
    /// <param name="prefab">The Copy of the gameObject which needs to be instantiated</param>
    /// <returns>The instance of the prefab or copy which was instantiated</returns>
    public static GameObject Instantiate(GameObject prefab)
    {
        Pool t;
        if (!pools.ContainsKey(prefab))
        {
            pools.Add(prefab,t = new Pool());

        }
        else
        {
            t = pools[prefab];
        }

        LinkedListNode<GameObject> node=t.InsertToPool(prefab);
        if(!nodes.ContainsKey(node.Value))
            nodes.Add(node.Value, new KeyValuePair<Pool, LinkedListNode<GameObject>>(t, node));
        return node.Value;
    }


    #region Instantiate Overloads

    public static GameObject Instantiate(GameObject prefab,Transform parent)
    {
        GameObject obj = Instantiate(prefab);

        obj.transform.parent = parent;
        return obj;
    }


    public static GameObject Instantiate(GameObject prefab,Vector3 position,Quaternion rotation)
    {
        GameObject obj = Instantiate(prefab);

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }
    public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation,Transform parent)
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.parent = parent;
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public static GameObject Instantiate(GameObject prefab, Transform parent,bool instantiateInWorldSpace)
    {
        GameObject obj = Instantiate(prefab);

        obj.transform.parent = parent;
        obj.transform.position = parent.transform.position;
        obj.transform.rotation = parent.transform.rotation;
        obj.transform.localScale = Vector3.one;
        return obj;
    }

    #endregion


    /// <summary>
    /// Gives access to object pool of the prefab
    /// </summary>
    public static Pool GetPool(GameObject prefab)
    {
        if (pools.ContainsKey(prefab))
            return pools[prefab];
        Pool t;
        pools.Add(prefab, t = new Pool());
        return t;
    }

    /// <param name="poolSize">Size of the object pool of this prefab.</param>
    public static GameObject Instantiate(GameObject prefab,int poolSize)
    {
        Pool t;
        if (!pools.ContainsKey(prefab))
        {
            pools.Add(prefab, t = new Pool());

        }
        else
        {
            t = pools[prefab];
        }
        t.poolSize = poolSize;
        return Instantiate(prefab);
    }


    /// <summary>
    /// Instantiate the object ignoring the pool limit it follows pool actions afterwards.
    /// </summary>
    public static GameObject InstantiateGrowing(GameObject prefab)
    {
        GameObject obj;
        Pool t;
        if (!pools.ContainsKey(prefab))
        {
            pools.Add(prefab,t=new Pool());
        }
        else
        {
            t = pools[prefab];
        }
        if (t.growing != true)
        {
            t.growing = true;
            obj = Instantiate(prefab);
            t.growing = false;
        }
        else
            obj = Instantiate(prefab);
        return obj;
    }


    /// <summary>
    /// Sets the PoolSize of the prefab
    /// </summary>
    public static void SetPoolSize(GameObject prefab,int size)
    {
        Pool t;
        if (!pools.ContainsKey(prefab))
        {
            pools.Add(prefab, t = new Pool());
        }
        else
        {
            t = pools[prefab];
        }
        t.poolSize = size;
    }

    /// <summary>
    /// Destroy a pool Object.(Sets it inactive until another pool object is to be instantiated).If not in pool it is destroyed normally.
    /// </summary>
    public static void Destroy(GameObject obj)
    {
        if (nodes.ContainsKey(obj))
            nodes[obj].Key.RemoveObjectFromPool(nodes[obj].Value);
        else
            Object.Destroy(obj);
    }

    public  class Pool
    {
        public bool growing = false;
        public int poolSize = 10;
        public int activeCount
        {
            get { return activeObjects.Count; }
        }

        public LinkedList<GameObject> activeObjects=new LinkedList<GameObject>();
        public LinkedList<GameObject> inactiveObjects = new LinkedList<GameObject>();


        public Pool()
        {
            poolSize = globalPoolSize;
        }

        public LinkedListNode<GameObject> InsertToPool(GameObject gameObject)
        {
            LinkedListNode<GameObject> node;
            if (!growing)
            {
                if (activeCount >= poolSize)
                    RemoveObjectFromPool();
            }

            if (inactiveObjects.Count > 0)
            {
                node = inactiveObjects.First;
                inactiveObjects.Remove(node);
                activeObjects.AddLast(node);
                node.Value.SetActive(true);
            }
            else
            {
                node = new LinkedListNode<GameObject>(Object.Instantiate(gameObject));
                activeObjects.AddLast(node);
            }
            return node;
        }
        public void RemoveObjectFromPool()
        {
            if (activeObjects.Count > 0)
            {
                activeObjects.First.Value.SetActive(false);
                LinkedListNode<GameObject> node = activeObjects.First;
                activeObjects.Remove(node);
                inactiveObjects.AddLast(node);
            }
        }
        public void RemoveObjectFromPool(LinkedListNode<GameObject> node)
        {
            activeObjects.Remove(node);
            inactiveObjects.AddLast(node);
            node.Value.SetActive(false);
        }
    }
}
