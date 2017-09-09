using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    public Text IP;

    public GameObject serverManagerPrefab;
    public GameObject clientManagerPrefab;

    public void StartServer()
    {
        SceneManager.LoadScene("Main");
        GameObject serverManager = Instantiate(serverManagerPrefab);
        serverManager.GetComponent<ServerManager>().IP = IP.text;
        DontDestroyOnLoad(serverManager);
    }

    public void StartClient()
    {
        SceneManager.LoadScene("Main");
        GameObject clientManager = Instantiate(clientManagerPrefab);
        clientManager.GetComponent<ClientManager>().IP = IP.text;
        DontDestroyOnLoad(clientManager);
    }
}
