using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TH
{
    public class Chips : MonoBehaviour
    {
        private Vector3 screenPoint;
        private Vector3 offset;
        private bool isCollided;
        private bool isClicked;

        public Text test;
        private int numberOfChip;
        public GameObject chip1_Prefab;
        public GameObject chip5_Prefab;
        public GameObject chip10_Prefab;

        // Use this for initialization
        void Start()
        {
            numberOfChip = gameObject.transform.childCount;
            GameObject tmp = Instantiate(chip1_Prefab);
            tmp.transform.SetParent(gameObject.transform);
            tmp.transform.localPosition = Vector3.zero;
            tmp.transform.localRotation = Quaternion.identity;
        }

        void OnMouseDown()
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            isClicked = true;
        }

        void OnMouseDrag()
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            //cursorPosition.y = gameObject.transform.position.y;
            transform.position = cursorPosition;
        }

        void OnMouseUp()
        {
            isClicked = false;
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Chips")
            {
                numberOfChip = gameObject.transform.childCount;
         
            }
        }

        void OnCollisionExit(Collision other)
        {
            var otherChips = other.gameObject.GetComponent<Chips>();
            var otherCollider = other.gameObject.GetComponent<BoxCollider>();

            if (other.gameObject.tag == "Chips" && isClicked)
            {
                Transform[] children = GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child.gameObject.tag == "Chip")
                    {
                        otherChips.numberOfChip++;
                        otherCollider.size += new Vector3(0, 2f, 0);
                        otherCollider.center += new Vector3(0, 1f, 0);
                        child.SetParent(other.gameObject.transform);
                        child.transform.localPosition = new Vector3(0, 2.0f * (otherChips.numberOfChip - 1), 0);
                        child.transform.localScale = Vector3.one;
                        child.transform.localRotation = Quaternion.identity;
                        Debug.Log("Hello", gameObject);
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}