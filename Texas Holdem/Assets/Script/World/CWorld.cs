using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TH {
public class CWorld : World {

    private ClientManager client_manager_;

    public ClientManager ClientManager
    {
        get { return client_manager_; }
        set { client_manager_ = value; }
    }

    public WorldSnapshot CurrSnapshot;

    public void ApplySnapshot(WorldSnapshot snapshot)
    {
        SyncWorld(snapshot);
    }

    private new void Awake()
    {
        base.Awake();
        CurrSnapshot = CreateWorldSnapshot();
    }

    private void Start()
    {
        client_manager_.AsyncReceiveSnapshot();
    }

    private void Update()
    {
        ApplySnapshot(CurrSnapshot);
    }

    protected void SyncWorld(WorldSnapshot snapshot)
    {
        foreach (var os in snapshot.Objects) {
            var obj = object_manager_.Find(os.object_id);
            if (obj == null) {
                obj = new Object(); 
                obj.world = this;

                switch (os.object_type) {
                case 1:
                    obj.gobj = Instantiate(CubePrefab);
                    break;
                default:
                    continue;
                }

                obj.object_id = os.object_id;
                obj.object_type = os.object_type;
                obj.possess_info = os.possess_info;

                object_manager_.Add(obj);
            }
            obj.gobj.transform.position = os.pos.Get();
        }
    }
}
}
