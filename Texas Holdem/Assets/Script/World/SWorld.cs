using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
public class SWorld : World {

    private ServerManager server_manager_;

    public ServerManager ServerManager {
        get { return server_manager_; }
        set { server_manager_ = value; }
    }

    private new void Awake()
    {
        base.Awake();
        Chips.ObjectManager = object_manager_;
        var chips = Instantiate(ChipsPrefab).GetComponent<Chips>();
        chips.SetChips(0, 3);
    }

    private void Update()
    {
        if (server_manager_ == null) return;
    }

    private void LateUpdate()
    {
        var snapshot = CreateWorldSnapshot();
        server_manager_.SendSnapshot(snapshot);
    }
}
}

