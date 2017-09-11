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
        Debug.Log("Awake()");
    }

    private void Update()
    {
        if (server_manager_ == null) return;

        Vector3 prev_loc = test_cube_.transform.position;
        prev_loc.x += 0.01f;
        test_cube_.transform.position = prev_loc;
    }

    private void LateUpdate()
    {
        var snapshot = CreateWorldSnapshot();
        // 
    }
}
}
