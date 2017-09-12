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
    }

    private void Update()
    {
        if (server_manager_ == null) return;

        Vector3 prev_loc = test_cube_.transform.position;
        prev_loc.x += Input.GetAxis("Horizontal") * 0.02f;
        prev_loc.y += Input.GetAxis("Vertical") * 0.02f;

        test_cube_.transform.position = prev_loc;
    }

    private void LateUpdate()
    {
        var snapshot = CreateWorldSnapshot();
        server_manager_.SendSnapshot(snapshot);
    }

    // have to handle input
}
}
