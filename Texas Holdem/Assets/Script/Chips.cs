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
        private bool isInit; 

        public GameObject chip1_Prefab;
        public GameObject chip5_Prefab;
        public GameObject chip10_Prefab;
        public GameObject newChips;

        // Use this for initialization
        void Start()
        {
            isInit = false;
        }

        void Update()
        {
            if (Input.GetKeyDown("x")&&!isInit)
            {
                isInit = true;
                GameObject tmp = Instantiate(chip1_Prefab);
                InitObject(tmp, Vector3.zero);
            }
        }

        void InitObject(GameObject obj, Vector3 pos)
        {
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = pos;
            obj.transform.localRotation = Quaternion.identity;
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
                GameObject a = SeperateChips(1);
            }

        }

        void OnCollisionEnter(Collision target)
        {
            if (target.gameObject.tag == "Chips")
            {
                numberOfChip = gameObject.transform.childCount;
            }
        }

        void OnCollisionExit(Collision target)
        {
            if (target.gameObject.tag == "Chips" && isClicked)
            {
                int n = gameObject.transform.childCount;
                MoveToTarget(target.gameObject, n);
                Destroy(gameObject);
            }
        }

        void MoveToTarget(GameObject target, int n)
        {
            var targetChips = target.GetComponent<Chips>();
            var targetCollider = target.GetComponent<BoxCollider>();
            Transform[] children = GetComponentsInChildren<Transform>();
            int loop = n;
            foreach (Transform child in children)
            {
                if (child.gameObject.tag == "Chip")
                {
                    if (--loop < 0) break;
                    targetChips.numberOfChip++;
                    child.SetParent(target.transform);
                    child.transform.localPosition = new Vector3(0, 2*(targetChips.numberOfChip - 1), 0);
                    child.transform.localScale = Vector3.one;
                    child.transform.localRotation = Quaternion.identity;
                    child.SetAsFirstSibling();
                }
            }
            targetCollider.size += new Vector3(0, 2*n, 0);
            targetCollider.center += new Vector3(0, n, 0);
        }

        GameObject SeperateChips(int n)
        {
            GameObject tmp = Instantiate(newChips);
            InitObject(tmp, new Vector3(1.5f, 0, 0));
            tmp.transform.SetParent(null);

            var sourceChips = gameObject.GetComponent<Chips>();
            var sourceCollider = gameObject.GetComponent<BoxCollider>();
            sourceChips.numberOfChip -= n;
            sourceCollider.size -= new Vector3(0, 2f * n, 0);
            sourceCollider.center -= new Vector3(0, n, 0);

            var targetCollider = tmp.GetComponent<BoxCollider>();
            targetCollider.center = new Vector3(0, -1, 0);
            targetCollider.size = new Vector3(1, 0, 1);

            MoveToTarget(tmp, n);
            return tmp;
        }
    }
}