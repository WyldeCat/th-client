using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace TH {
public class ServerManager : MonoBehaviour {

    public string IP { get { return IP_; } set { IP_ = value; } }
    public short Port { get { return port_; } set { port_ = value; } }

    private string IP_;
    private short port_;

    private TcpListener listener_ = null;
    private TcpClient client_ = null;

    private SWorld world_;
    private bool is_init_ = false;
    private bool is_connected_ = false;

    public void InitNetwork()
    {
        listener_ = new TcpListener(IPAddress.Parse(IP_), port_);
        listener_.Start();
        listener_.BeginAcceptTcpClient(
            new AsyncCallback(OnClientAccept),
            listener_);
    }

    public void SendSnapshot(WorldSnapshot snapshot)
    {
        // HACK about async accept
        if (!is_connected_) return;

        byte[] byteArr = snapshot.W2ByteArray();
        byte[] header = System.BitConverter.GetBytes(
            (ushort)byteArr.Length);
        client_.Client.BeginSend(header, 0,
            header.Length, SocketFlags.None, null, null);
            
        client_.Client.BeginSend(byteArr, 0,
            byteArr.Length, SocketFlags.None, null, null);
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnMainFinishedLoading;
    }

    private void OnMainFinishedLoading(Scene scene,
        LoadSceneMode mode)
    {
        world_ = GameObject.Find("SWorld").GetComponent<SWorld>();
        world_.ServerManager = this;
        is_init_ = true;
    }
    
    private void OnClientAccept(IAsyncResult ar)
    {
        client_ = listener_.EndAcceptTcpClient(ar);
        is_connected_ = true;
        Debug.Log("Client Accepted");
    }
}
}
