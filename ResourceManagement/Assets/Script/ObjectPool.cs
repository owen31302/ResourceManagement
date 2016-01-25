using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool {

    class ObjectPoolData {
        public GameObject m_gameObject;
        public bool m_isUsing;

        public ObjectPoolData() {
            m_isUsing = false;
        }
    }

    int m_howManyTypes;
    List<ObjectPoolData>[] m_containers;
    static public ObjectPool m_instance;

    public ObjectPool() {
        m_howManyTypes = 10;
        m_containers = new List<ObjectPoolData>[m_howManyTypes];
    }

    public int get_howManyTypes() {
        return m_howManyTypes;
    }

    //找到空缺的記憶體
    int FindEmptySlot() {
        int slotIndex;
        for (slotIndex = 0; slotIndex < m_howManyTypes; slotIndex++) {
            if (m_containers[slotIndex] == null) {
                break;
            }
        }
        // 防呆
        if (slotIndex == m_howManyTypes)
        {
            return -1;
        }
        else {
            return slotIndex;
        }
    }

    // 實體化物件
    public int InstantiateObjectsInObjectPool(Object obj, int amounts) {
        // 找到空的容器
        int slotIndex = FindEmptySlot();
        // 實體化物件，並且將物件丟入容器管理
        m_containers[slotIndex] = new List<ObjectPoolData>();
        GameObject tempGameObject;
        ObjectPoolData tempObjectPoolData;
        for (int i = 0; i < amounts; i++) {
            tempGameObject = MonoBehaviour.Instantiate(obj) as GameObject;
            setShowHideObject(tempGameObject, false);
            tempObjectPoolData = new ObjectPoolData();
            tempObjectPoolData.m_gameObject = tempGameObject;
            m_containers[slotIndex].Add(tempObjectPoolData);
        }
        // 回傳容器編碼
        return slotIndex;
    }

    void setShowHideObject(GameObject gameObject, bool setShow) {
            gameObject.SetActive(setShow);
    }

    //把容器中，沒有用到的obj拿出來
    public GameObject getUnusedObjectFromObjectPool(int slotIndex) {
        // 防呆
        if (checkIndex(slotIndex)) return null;
        // 邏輯
        int i;
        for (i = 0; i < m_containers[slotIndex].Count; i++) {
            if (m_containers[slotIndex][i].m_isUsing == false) {
                setShowHideObject(m_containers[slotIndex][i].m_gameObject, true);
                m_containers[slotIndex][i].m_isUsing = true;
                break;
            }
        }
        // 防呆
        if (i == m_containers[slotIndex].Count)
        {
            return null;
        }
        else {
            return m_containers[slotIndex][i].m_gameObject;
        }
    }

    public void DestroyObjectsInObjectPoolAtSlot(int slotIndex) {
        // 防呆
        if (checkIndex(slotIndex)) return;
        // 邏輯
        for (int i = 0; i < m_containers[slotIndex].Count; i++) {
            MonoBehaviour.Destroy(m_containers[slotIndex][i].m_gameObject);
            m_containers[slotIndex][i] = null;
        }
        m_containers[slotIndex] = null;
    }

    public bool ReleaseUsedObjectToPool(GameObject obj, int slotIndex)
    {
        // 防呆
        if (checkIndex(slotIndex)) return false;
        // 邏輯
        for (int i = 0; i < m_containers[slotIndex].Count; i++)
        {
            if (m_containers[slotIndex][i].m_gameObject == obj) {
                setShowHideObject(m_containers[slotIndex][i].m_gameObject, false);
                m_containers[slotIndex][i].m_isUsing = false;
                return true;
            }
        }
        return false;
    }

    bool checkIndex(int slotIndex) {
        // 防呆
        if (slotIndex < 0 || slotIndex >= m_howManyTypes)
        {
            return true;
        }
        return false;
    }
}
