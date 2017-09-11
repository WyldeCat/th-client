
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TH
{

    public class Spawn : MonoBehaviour {

        public GameObject prefab;
        public Button spawnBtn;

        // Use this for initialization
        void Start() {
            Button btn = spawnBtn.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick()
        {
            Instantiate(prefab);
        }
    }
}