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
        
        // HACK
        HostId = 1;

        Chips.ObjectManager = object_manager_;
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
                switch (os.object_type) {
                case 1:
                    var chips = Instantiate(ChipsPrefab).
                        GetComponent<Chips>();
                    obj = chips.Object;
                    chips.Object.Id = os.object_id;
                    chips.Object.PossessInfo =
                        os.possess_info;
                    break;
                default:
                    continue;
                }
            }

            var collider = obj.gobj.GetComponent<Collider>();
            if (collider != null) {
                Debug.Log("Collider disabled");
                collider.enabled = false;
            }

            obj.snapshot_handler(os);
        }

        int[]list = new int[30];
        int count = 0;

        foreach (var kv in object_manager_.Dictionary) {
            bool is_exist = false;
            foreach (var os in snapshot.Objects) {
                if (os.object_id == kv.Value.object_id) {
                    is_exist = true;
                    break;
                }
            }
            if (!is_exist) {
                list[count++] = kv.Value.object_id;
                Destroy(kv.Value.gobj);
            }
        }

        for (var i = 0; i < count; i++) {
            object_manager_.Delete(list[i]);
        }
    }
}
}
