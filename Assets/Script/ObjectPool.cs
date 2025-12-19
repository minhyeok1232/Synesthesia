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

    public List<Queue<GameObject>> noteQueues = new List<Queue<GameObject>>();
    
    void Awake() // Start보다 먼저 인스턴스를 설정하여 다른 스크립트가 접근 가능하도록 합니다.
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    void Start()
    {
        // ObjectInfo 배열의 크기만큼 반복합니다 (4키면 4번)
        for(int i = 0; i < objectInfo.Length; i++)
        {
            // 각 라인(i)별로 노트 큐를 생성하고 초기화합니다.
            Queue<GameObject> newQueue = InsertQueue(objectInfo[i]);
            noteQueues.Add(newQueue);
        }
    }

    private Queue<GameObject> InsertQueue(ObjectInfo _objectInfo)
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < _objectInfo.count; i++)
        {
            GameObject clone = Instantiate(_objectInfo.prefab, transform.position, Quaternion.identity);
            clone.SetActive(false);
            if (_objectInfo.parent != null)
                clone.transform.SetParent(_objectInfo.parent, false);
            else
                clone.transform.SetParent(this.transform, false);

            queue.Enqueue(clone);
        }

        return queue;
    }
    
    // 외부에서 노트를 가져갈 때 사용할 메서드
    public GameObject GetNote(int lineID)
    {
        if (lineID >= 0 && lineID < noteQueues.Count && noteQueues[lineID].Count > 0)
        {
            return noteQueues[lineID].Dequeue();
        }
        
        // 큐가 비었을 경우 새로 생성하거나 null을 반환합니다. (오브젝트 풀링 방식에 따라 다름)
        // 현재는 간단하게 null 반환
        return null; 
    }
    
    // 노트를 반납할 때 사용할 메서드
    public void ReturnNote(GameObject note, int lineID)
    {
        if (lineID >= 0 && lineID < noteQueues.Count)
        {
            note.SetActive(false);
            note.transform.SetParent(objectInfo[lineID].parent, false); // 원래 부모로 복귀
            noteQueues[lineID].Enqueue(note);
        }
    }
}
