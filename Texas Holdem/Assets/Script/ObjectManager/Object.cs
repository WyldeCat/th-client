using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
// wrapper of GameObject in TH
public class Object {
    public delegate object SnapshotProducer();
    public delegate void SnapshotHandler(object snapshot);

    public World world;

    public GameObject gobj;

    public int object_id;
    public int object_type;
    public int possess_info;

    public SnapshotProducer snapshot_producer;
    public SnapshotHandler snapshot_handler;
}
}
