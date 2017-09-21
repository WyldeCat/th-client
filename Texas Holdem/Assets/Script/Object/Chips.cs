using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TH
{
public class Chips : MonoBehaviour
{
    private static int lastChipId = 0;
    //private Vector3 BOXSIZE = new Vector3(0, 2, 0);
    //private Vector3 BOXCENTER = new Vector3(0, 1, 0);

    private Vector3 screenPoint;
    private Vector3 offset;
    private bool isClicked;
    private int numberOfChip;
    private int value;

    private Object obj;
    private ObjectManager objectManager;

    [SerializeField]
    private GameObject [] chipPrefabs;
    [SerializeField]
    private GameObject newChips;

    public Object Object {
        get {
            return obj;
        }
    }

    public ObjectManager ObjectManager {
        get {
            return objectManager;
        }
        set {
            objectManager = value;
        }
    }

    void Awake()
    {
        obj = new Object();
        obj.gobj = gameObject;
        obj.object_type = 1; //Chips
        obj.snapshot_producer = GetObjSnapshot;
        obj.snapshot_handler = SyncChips;
    }

    void InitObject(GameObject obj, Vector3 pos)
    {
        obj.transform.SetParent(gameObject.transform);
        obj.transform.localPosition = pos;
        obj.transform.localRotation = Quaternion.identity;
        Object.Id = (++lastChipId);
        Object.object_type = 1;
        objectManager = ObjectManager;
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
                if (MoveToTarget(target.gameObject, n))
                {
                    objectManager.Delete(Object.Id);
                    Destroy(gameObject);
                }
            }
        }

        void RegulateCollider(BoxCollider col, int num)
        {
            //col.size += BOXSIZE * num;
            //col.center += BOXCENTER * num;
            col.size += new Vector3(0,2,0) * num;
            col.center += new Vector3(0,1,0) * num;
        }

        bool MoveToTarget(GameObject target, int num)
        {
            var targetChips = target.GetComponent<Chips>();
            if (targetChips.value != value)
                return false;
            var sourceChips = GetComponent<Chips>();

            var targetCollider = target.GetComponent<BoxCollider>();
            var sourceCollider = GetComponent<BoxCollider>();

            Transform[] children = GetComponentsInChildren<Transform>();
            int loop = num;
            foreach (Transform child in children)
            {
                if (child.gameObject.tag == "Chip")
                {
                    if (--loop < 0) break;
                    targetChips.numberOfChip++;
                    child.SetParent(target.transform);
                    child.transform.localPosition = new Vector3(0, 2 * (targetChips.numberOfChip - 1), 0);
                    child.transform.localScale = Vector3.one; //
                    child.transform.localRotation = Quaternion.identity;
                    child.SetAsFirstSibling();
                }
            }
            RegulateCollider(sourceCollider, -num);
            RegulateCollider(targetCollider, num);
            return true;
        }

        public GameObject SeperateChips(int n)
        {
            GameObject tmp = Instantiate(newChips);
            InitObject(tmp, new Vector3(1.5f, 0, 0));
            tmp.transform.SetParent(null);

            var targetCollider = tmp.GetComponent<BoxCollider>();
            targetCollider.center = new Vector3(0, -1, 0);
            targetCollider.size = new Vector3(1, 0, 1);

            bool moveSuccess = MoveToTarget(tmp, n);
            return tmp;
        }

        public void SetChips(int value, int num)
        {
            for (int i = 0; i < num; ++i)
            {
                GameObject chip = Instantiate(chipPrefabs[value]);
                numberOfChip++;
                chip.transform.SetParent(gameObject.transform);
                chip.transform.localPosition = new Vector3(0, 2 * (numberOfChip - 1), 0);
                chip.transform.localRotation = Quaternion.identity;
                chip.transform.SetAsFirstSibling();
            }
            var collider = GetComponent<BoxCollider>();
            RegulateCollider(collider, num-1);
        }
            
        /*
        void OnCollisionExit(Collision target)
        {
            if (target.gameObject.tag == "Chips" && isClicked)
            {
                int n = gameObject.transform.childCount;
                if(MoveToTarget(target.gameObject, n))
                    Destroy(gameObject);
            }
        }

        bool MoveToTarget(GameObject target, int n)
        {
            var targetChips = target.GetComponent<Chips>();
            if (targetChips.value != value)
                return false;
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
            return true;
        }

        public GameObject SeperateChips(int n)
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

            bool moveSuccess = MoveToTarget(tmp, n);
            return tmp;
        }

        public void SetChips(int value, int num)
        {
            if (num > numberOfChip)
            {
                for (int i = num-numberOfChip; i < num; ++i)
                {
                    GameObject chip = Instantiate(chipPrefabs[value]);
                    numberOfChip++;
                    chip.transform.SetParent(gameObject.transform);
                    chip.transform.localPosition = new Vector3(0, 2 * (numberOfChip - 1), 0);
                    chip.transform.localRotation = Quaternion.identity;
                    chip.transform.SetAsFirstSibling();
                }
                var col = GetComponent<BoxCollider>();
                col.size += new Vector3(0, 2 * (num - 1), 0);
                col.center += new Vector3(0, num - 1, 0);
            }
            else{
                for(int i=0;)

        }*/
                    
    ObjectSnapshot GetObjSnapshot()
    {
        var objSnapshot = new ChipsSnapshot();
        objSnapshot.object_id = obj.object_id;
        objSnapshot.object_type = obj.object_type;
        objSnapshot.pos.Set(gameObject.transform.position);
        objSnapshot.height = numberOfChip;
        objSnapshot.value = value;
        return objSnapshot as ObjectSnapshot;
    }
    
    void SyncChips(ObjectSnapshot objSnapshot)
    {
        var syncInfo = objSnapshot as ChipsSnapshot;
        SetChips(syncInfo.value, syncInfo.height - numberOfChip);
        gameObject.transform.position = syncInfo.pos.Get();
        value = syncInfo.value;
        numberOfChip = syncInfo.height;
    }
}
}

