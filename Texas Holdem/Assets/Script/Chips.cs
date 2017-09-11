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

        public Text test;
        private int height;


        void SetHeight(int h)
        {
            height = h;
        }

        int GetHeight()
        {
            return height;
        }

        // Use this for initialization
        void Start()
        {
            height = 1;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseDown()
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            isCollided = false;
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
            if (isCollided == true)
            {
                Destroy(this.gameObject);
            }
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Chips")
            {
                isCollided = true;
            }
        }

        void OnCollisionExit(Collision other)
        {
            isCollided = false;

        }
    }
}