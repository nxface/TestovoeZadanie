using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public GameObject Pobj;
    public int Amount;

    List<GameObject> pObjects;

    void Start()
    {
        pObjects = new List<GameObject>();
        for (int i = 0; i < Amount; ++i)
        {
            GameObject o = Instantiate(Pobj); o.SetActive(false);
            o.transform.SetParent(this.transform);
            pObjects.Add(o); 
        } 
    }

    public GameObject GetPoolObj()
    {
        for (int i = 0; i < pObjects.Count; i++)
        {
            if (!pObjects[i].activeInHierarchy)
                return pObjects[i];
        }
        GameObject obj = Instantiate(Pobj);obj.SetActive(false);
        obj.transform.SetParent(this.transform);
        pObjects.Add(obj);
        return obj;
    }

}
