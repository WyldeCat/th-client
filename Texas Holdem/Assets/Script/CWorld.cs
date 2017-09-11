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
}
}
