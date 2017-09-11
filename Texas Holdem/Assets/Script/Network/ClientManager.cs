using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace TH {
public class ClientManager : MonoBehaviour {

    public string IP { get { return IP_; } set { IP_ = value; } }
    public short Port { get { return port_; } set { port_ = value; } }

    private string IP_;
    private short port_;

    private TcpClient server_;
    // HACK
    private byte[] buffer_ = new byte[1024];
    private CWorld world_;
    private bool is_init_ = false;
    private bool is_connected_ = false;

    public void InitNetwork()
    {
        server_ = new TcpClient();
        var dummy = new WorldSnapshot();
        try
        {
            server_.Connect(new IPEndPoint(IPAddress.Parse(IP_), port_));
            is_connected_ = true;
        }
        catch
        {
        }
    }
    
    public void AsyncReceiveSnapshot()
    {
        if (!is_connected_) return;
        // assume well connected to server
        server_.Client.BeginReceive(buffer_, 0, 1024, SocketFlags.None,
            new System.AsyncCallback(OnSnapshotReceive), null);
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

    private void OnSnapshotReceive(System.IAsyncResult iar)
    {
        int n = server_.Client.EndReceive(iar);
        if (n == 0) {
            is_connected_ = false;
            Debug.Log("Connection closed.");
            return;
        }

        var snapshot = WorldSnapshot.B2WorldSnapshot(buffer_);
        world_.CurrSnapshot = snapshot;
        AsyncReceiveSnapshot();
    }
}
}
