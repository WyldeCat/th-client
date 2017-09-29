using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TH
{
public class Chips : MonoBehaviour
{
    private static int lastChipId_ = 0;

    private Vector3 screenPoint_;
    private Vector3 offset_;
    private bool isClicked_;
    private int numberOfChip_;
    private int value_;

    private Object obj_;
    private static ObjectManager objectManager_;

    [SerializeField]
    private GameObject [] chipPrefabs_;
    [SerializeField]
    private GameObject newChips_;

    public Object Object {
        get {
            return obj_;
        }
    }

    public static ObjectManager ObjectManager {
        get {
            return objectManager_;
        }
        set {
            objectManager_ = value;
        }
    }

    void Awake()
    {
        obj_ = new Object();
        obj_.gobj = gameObject;
        obj_.object_type = 1;
        obj_.Id = (lastChipId_++);
        ObjectManager.Add(obj_);
        obj_.snapshot_producer = GetObjSnapshot;
        obj_.snapshot_handler = SyncChips;
    }

    void InitObject(GameObject gobj, Vector3 pos)
    {
        gobj.transform.SetParent(gameObject.transform);
        gobj.transform.localPosition = pos;
        gobj.transform.localRotation = Quaternion.identity;
    }

    void OnMouseDown()
    {
        screenPoint_ = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset_ = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint_.z));
        isClicked_ = true;
    }

    void OnMouseDrag()
    {
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint_.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset_;
        transform.position = cursorPosition;
    }

    void OnMouseUp()
    {
        isClicked_ = false;
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
            numberOfChip_ = gameObject.transform.childCount;
        }
    }

        void OnCollisionExit(Collision target)
        {
            if (target.gameObject.tag == "Chips" && isClicked_)
            {
                int n = gameObject.transform.childCount;
                if (MoveToTarget(target.gameObject, n))
                {
                    objectManager_.Delete(Object.Id);
                    Destroy(gameObject);
                }
            }
        }

        void RegulateCollider(BoxCollider col, int num)
        {
            col.size += new Vector3(0,2,0) * num;
            col.center += new Vector3(0,1,0) * num;
        }

        bool MoveToTarget(GameObject target, int num)
        {
            var targetChips = target.GetComponent<Chips>();
            if (targetChips.value_ != value_)
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
                    targetChips.numberOfChip_++;
                    child.SetParent(target.transform);
                    child.transform.localPosition = new Vector3(0, 2 * (targetChips.numberOfChip_ - 1), 0);
                    child.transform.localScale = Vector3.one; 
                    child.transform.localRotation = Quaternion.identity;
                    child.SetAsFirstSibling();
                }
            }
            sourceChips.numberOfChip_ -= num;
            RegulateCollider(sourceCollider, -num);
            RegulateCollider(targetCollider, num);
            return true;
        }

        public GameObject SeperateChips(int n)
        {
            GameObject tmp = Instantiate(newChips_);
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
            if (num == 0) return;
            if (num > 0)
            {
                for (int i = 0; i < num; ++i)
                {
                    GameObject chip = Instantiate(chipPrefabs_[value]);
                    chip.transform.SetParent(gameObject.transform);
                    numberOfChip_++;
                    chip.transform.localPosition = new Vector3(0, 2 * (numberOfChip_ - 1), 0);
                    chip.transform.localRotation = Quaternion.identity;
                    chip.transform.SetAsFirstSibling();
                }
                var collider = GetComponent<BoxCollider>();
                RegulateCollider(collider, num - 1);
            }
            else {
                Transform[] children = GetComponentsInChildren<Transform>();
                var collider = GetComponent<BoxCollider>();
                RegulateCollider(collider, num);
                foreach(Transform child in children){
                    if (num == 0) break;
                    if (child.CompareTag("Chip"))
                    {
                        Destroy(child.gameObject);
                        num++;
                    }
                }
            }
        }
        
                    
    ObjectSnapshot GetObjSnapshot()
    {
        var objSnapshot = new ChipsSnapshot();
        objSnapshot.object_id = obj_.object_id;
        objSnapshot.object_type = obj_.object_type;
        objSnapshot.pos.Set(gameObject.transform.position);
        objSnapshot.height = numberOfChip_;
        objSnapshot.value = value_;
        return objSnapshot as ObjectSnapshot;
    }
    
    void SyncChips(ObjectSnapshot objSnapshot)
    {
        var syncInfo = objSnapshot as ChipsSnapshot;
        if(syncInfo.height != numberOfChip_)
            SetChips(syncInfo.value, syncInfo.height - numberOfChip_);
        gameObject.transform.position = syncInfo.pos.Get();
        value_ = syncInfo.value;
        numberOfChip_ = syncInfo.height;
    }
}
}

