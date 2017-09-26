using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
// wrapper of GameObject in TH
public class Object {
    public delegate ObjectSnapshot SnapshotProducer();
    public delegate void SnapshotHandler(ObjectSnapshot snapshot);

    public GameObject gobj;

    public int object_id;
    public int object_type;

    private int possess_info;
    // 0 is for server, 1 is for client

    public bool IsMine;
    
    public SnapshotProducer snapshot_producer;
    public SnapshotHandler snapshot_handler;

    public int Id {
        get {
            return object_id;
        }
        set {
            object_id = value;
        }
    }

    public int PossessInfo {
        get {
            return possess_info;
        }
        set {
            possess_info = value;
            IsMine = (World.HostId == possess_info);
        }
    }
    
}
}
