﻿using System.Collections;
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

        // HACK
        HostId = 0;

        Chips.ObjectManager = object_manager_;

        var chips1 = Instantiate(ChipsPrefab, new Vector3(0, 0, -3),
            Quaternion.identity).GetComponent<Chips>();
        var chips2 = Instantiate(ChipsPrefab, new Vector3(2, 0, 3),
            Quaternion.identity).GetComponent<Chips>();

        chips1.SetChips(0, 4);
        chips1.Object.PossessInfo = 0;

        chips2.SetChips(0, 3);
        chips2.Object.PossessInfo = 1;
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

