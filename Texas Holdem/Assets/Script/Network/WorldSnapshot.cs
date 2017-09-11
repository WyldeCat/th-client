using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

namespace TH {
[Serializable]
public class WorldSnapshot {

    public Vector3 TestCubePos;

    byte[] W2ByteArray()
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        bf.Serialize(ms, this);
        return ms.ToArray();
    }

    static WorldSnapshot B2WorldSnapshot(byte[] byteArr)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(byteArr);

        return bf.Deserialize(ms) as WorldSnapshot;
    }
}
}
