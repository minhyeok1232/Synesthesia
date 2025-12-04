using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectInfo
{
    public GameObject prefab;
    public int count;
    public Transform parent;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    
    [SerializeField] private ObjectInfo[] objectInfo = null;

    public Queue<GameObject> noteQueue = new Queue<GameObject>();
    
    void Start()
    {
        instance = this;
        noteQueue = InsertQueue(objectInfo[0]);
    }

    private Queue<GameObject> InsertQueue(ObjectInfo _objectInfo)
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < _objectInfo.count; i++)
        {
            GameObject clone = Instantiate(_objectInfo.prefab, transform.position, Quaternion.identity);
            clone.SetActive(false);
            if (_objectInfo.parent != null)
                clone.transform.SetParent(_objectInfo.parent);
            else
                clone.transform.SetParent(this.transform);

            queue.Enqueue(clone);
        }

        return queue;
    }
}
