using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
public class SWorld : World {

    private ServerManager server_manager_;
    private Object test_cube_;

    public ServerManager ServerManager {
        get { return server_manager_; }
        set { server_manager_ = value; }
    }

    private new void Awake()
    {
        base.Awake();
        test_cube_ = new Object();

        test_cube_.gobj = Instantiate(TestCubePrefab);
        test_cube_.world = this;
        test_cube_.object_id = 1;
        test_cube_.object_type = 1;
        test_cube_.possess_info = 1;

        object_manager_.Add(test_cube_);
    }

    private void Update()
    {
        if (server_manager_ == null) return;

        Vector3 prev_loc = test_cube_.gobj.transform.position;

        prev_loc.x += Input.GetAxis("Horizontal") * 0.02f;
        prev_loc.y += Input.GetAxis("Vertical") * 0.02f;

        test_cube_.gobj.transform.position = prev_loc;
    }

    private void LateUpdate()
    {
        var snapshot = CreateWorldSnapshot();
        server_manager_.SendSnapshot(snapshot);
    }

    // have to handle input
}
}
