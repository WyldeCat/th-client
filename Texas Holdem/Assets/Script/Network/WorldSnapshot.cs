using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

namespace TH {
[Serializable]
public class WorldSnapshot {

    public Vector3S TestCubePos;

    public byte[] W2ByteArray()
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        bf.Serialize(ms, this);
        return ms.ToArray();
    }

    public static WorldSnapshot B2WorldSnapshot(byte[] byteArr)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(byteArr);

        return bf.Deserialize(ms) as WorldSnapshot;
    }
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
