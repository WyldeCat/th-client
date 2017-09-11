using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientManager : MonoBehaviour {

    public string IP { get { return IP_; } set { IP_ = value; } }
    public short Port { get { return port_; } set { port_ = value; } }

    private string IP_;
    private short port_;

    private Socket server_;

    private CWorld world_;
    private bool is_init_ = false;

    // test vector
    private Vector3 test_vector_;
    public Vector3 GetTestVector()
    {
        return test_vector_;
    }

    public void InitNetwork()
    {
        server_ = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.IP);
        server_.Connect(new IPEndPoint(IPAddress.Parse(IP_), port_));
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnMainFinishedLoading;
    }

    private void OnMainFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        world_ = GameObject.Find("CWorld").GetComponent<CWorld>();
        world_.ClientManager = this;
        is_init_ = true;
    }
}
