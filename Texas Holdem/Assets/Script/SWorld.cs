using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWorld : MonoBehaviour {

    private ServerManager server_manager_;

    public ServerManager ServerManager {
        get { return server_manager_; }
        set { server_manager_ = value; }
    }

    private void Update()
    {
        if (server_manager_ == null) return;

        // test codes

        Vector3 prev_loc = transform.GetChild(0).position;
        prev_loc.x += 0.01f;
        transform.GetChild(0).position = prev_loc;
        server_manager_.SetTestVector(prev_loc);
    }
}
