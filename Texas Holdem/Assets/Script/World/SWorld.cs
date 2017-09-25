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

        var gchip1 = Instantiate(ChipsPrefab);
        var gchip2 = Instantiate(ChipsPrefab);

        var chips1 = gchip1.GetComponent<Chips>();
        var chips2 = gchip2.GetComponent<Chips>();

        gchip1.transform.position = new Vector3(0, 0, -3);
        gchip2.transform.position = new Vector3(2, 0, 3);

        chips1.SetChips(0, 4);
        chips2.SetChips(0, 3);
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

