using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviour {

    private string IP_;

    public string IP { set { IP_ = value; } }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnMainFinishedLoading;
    }

    private void OnMainFinishedLoading(Scene scene, LoadSceneMode mode)
    {
    }
}
