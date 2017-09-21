using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
public class test : MonoBehaviour {

        public GameObject chipsFrame;
    // Use this for initialization
    void Start() {
            GameObject tmp = Instantiate(chipsFrame);
            Chips chips = tmp.GetComponent<Chips>();
            chips.SetChips(1, 5);

    }

    // Update is called once per frame
    void Update() {

    }
}
}
