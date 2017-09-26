﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
public abstract class World : MonoBehaviour {

    public GameObject ChipsPrefab;
    public GameObject CubePrefab;

    protected ObjectManager object_manager_;

    public static int HostId = 0x7fffffff;

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
