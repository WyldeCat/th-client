using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
public class World : MonoBehaviour {

    public GameObject TestCubePrefab;

    protected GameObject test_cube_;
    protected ObjectManager object_manager_;

    protected void Awake()
    {
       test_cube_ = Instantiate(TestCubePrefab);
    }

    protected WorldSnapshot CreateWorldSnapshot()
    {
       WorldSnapshot snapshot = new WorldSnapshot(); 
       snapshot.TestCubePos.Set(test_cube_.transform.position);

       return snapshot;
    }
}
}
