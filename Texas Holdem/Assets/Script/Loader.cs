using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TH {
public class Loader : MonoBehaviour {

    public Text IP;
    public Text Port;

    public GameObject serverManagerPrefab;
    public GameObject clientManagerPrefab;

    public void StartServer()
    {
        SceneManager.LoadScene("ServerScene");
        
        GameObject serverManager = Instantiate(serverManagerPrefab);
        serverManager.GetComponent<ServerManager>().IP = IP.text;
        serverManager.GetComponent<ServerManager>().Port = 4000;
            // System.Convert.ToInt16(Port.text);
        serverManager.GetComponent<ServerManager>().InitNetwork();
        DontDestroyOnLoad(serverManager);
    }

    public void StartClient()
    {
        SceneManager.LoadScene("ClientScene");

        GameObject clientManager = Instantiate(clientManagerPrefab);
        clientManager.GetComponent<ClientManager>().IP = IP.text;
        clientManager.GetComponent<ClientManager>().Port = 4000;
            // System.Convert.ToInt16(Port.text);
        clientManager.GetComponent<ClientManager>().InitNetwork();
        DontDestroyOnLoad(clientManager);
    }
}
}
