using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TH {
public class CWorld : MonoBehaviour {

    private ClientManager client_manager_;

    public ClientManager ClientManager
    {
        get { return client_manager_; }
        set { client_manager_ = value; }
    }

    private void Update()
    {
        if (client_manager_ == null) return;

        // test codes
    }
}
}
