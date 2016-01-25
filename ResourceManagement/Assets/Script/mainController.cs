using UnityEngine;
using System.Collections;

public class mainController : MonoBehaviour {

    public GameObject prefab1;

    void Awake() {
        ObjectPool.m_instance = new ObjectPool();
        //Debug.Log(ObjectPool.m_instance.get_howManyTypes());
        
    }

	// Use this for initialization
	void Start () {
        ObjectPool.m_instance.InstantiateObjectsInObjectPool(prefab1, 10);
        
    }

    GameObject mGameObject = null;

    // Update is called once per frame
    void Update () {
        
        if (Input.GetMouseButtonDown(1))
        {
            mGameObject = ObjectPool.m_instance.getUnusedObjectFromObjectPool(0);
        }
        else if (Input.GetMouseButtonDown(0)) {
            if (ObjectPool.m_instance.ReleaseUsedObjectToPool(mGameObject, 0))
            {
                mGameObject = null;
                Debug.Log("Release OK");
            }
            else {
                Debug.Log("Release fail");
            }
        }
	}
}
