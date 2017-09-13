using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
public class World : MonoBehaviour {

    public GameObject TestCubePrefab;

    protected ObjectManager object_manager_;

    protected void Awake()
    {
        object_manager_ = new ObjectManager();
    }

    protected WorldSnapshot CreateWorldSnapshot()
    {
       WorldSnapshot snapshot = new WorldSnapshot(); 
       snapshot.ObjectNum = object_manager_.Count;
       snapshot.Objects = object_manager_.CreateObjectsSnapshot();
       return snapshot;
    }
}
}
