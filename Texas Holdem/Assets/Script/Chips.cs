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
        private int numberOfChip;

        public Text test;
        public GameObject chip1_Prefab;
        public GameObject chip5_Prefab;
        public GameObject chip10_Prefab;
        public GameObject newChips;

        void InitObject(GameObject obj, Vector3 pos)
        {
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = pos;
            obj.transform.localRotation = Quaternion.identity;
        }

        // Use this for initialization
        void Start()
        {
            numberOfChip = gameObject.transform.childCount;
            GameObject tmp = Instantiate(chip1_Prefab);
            InitObject(tmp, Vector3.zero);
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

        void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1) && gameObject.transform.childCount > 1)
            {
                GameObject tmp = Instantiate(newChips);
                InitObject(tmp, new Vector3(1.5f, 0, 0));
                tmp.transform.SetParent(null);

                var chips = gameObject.GetComponent<Chips>();
                var collider = gameObject.GetComponent<BoxCollider>();

                Transform child = gameObject.transform.GetChild(0);
                child.SetParent(null);
                chips.numberOfChip--;
                collider.size -= new Vector3(0, 2f, 0);
                collider.center -= new Vector3(0, 1f, 0);
                Destroy(child.gameObject);
          
            }
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
                        child.SetAsFirstSibling();
                        Debug.Log("Hello", gameObject);
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}