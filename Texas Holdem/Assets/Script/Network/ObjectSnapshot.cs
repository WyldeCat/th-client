using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace TH {
[Serializable]
public class ObjectSnapshot {
    public int object_id;
    public int object_type;
    public int possess_info;
    public Vector3S pos;
}
    
[Serializable]
public struct Vector3S {
    public float x;
    public float y;
    public float z; 
    
    public void Set(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }

    public Vector3 Get()
    {
        return new Vector3(x, y, z);
    }
}
}
