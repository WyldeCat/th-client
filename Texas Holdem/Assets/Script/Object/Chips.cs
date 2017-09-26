using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TH {
public class Chips : MonoBehaviour {
    private static int lastChipId = 0;
    public int id;

    private Vector3 screenPoint;
    private Vector3 offset;
    private bool isClicked;
    private int numberOfChip;
    private int value;

    private Object obj;
    private static ObjectManager objectManager;

    [SerializeField]
    private GameObject [] chipPrefabs;
    [SerializeField]
    private GameObject newChips;

    public Object Object {
        get {
            return obj;
        }
    }

    public static ObjectManager ObjectManager {
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
        obj.object_type = 1;
        obj.PossessInfo = -1;
        obj.Id = (lastChipId++);
        id = obj.Id;
        ObjectManager.Add(obj);
        obj.snapshot_producer = GetObjSnapshot;
        obj.snapshot_handler = SyncChips;
    }

    void InitObject(GameObject gobj, Vector3 pos)
    {
        gobj.transform.SetParent(gameObject.transform);
        gobj.transform.localPosition = pos;
        gobj.transform.localRotation = Quaternion.identity;
    }

    void OnMouseDown()
    {
        if (!obj.IsMine) {
            return;
        }

        screenPoint =
            Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position -
            Camera.main.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x, Input.mousePosition.y, screenPoint.z
            ));
        isClicked = true;
    }

    void OnMouseDrag()
    {
        if (!obj.IsMine) {
            return;
        }

        Vector3 cursorPoint = new Vector3(
            Input.mousePosition.x,Input.mousePosition.y,screenPoint.z
        );

        Vector3 cursorPosition =
            Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        transform.position = cursorPosition;
    }

    void OnMouseUp()
    {
        if (!obj.IsMine) {
            return;
        }
        isClicked = false;
    }

    void OnMouseOver()
    {
        if (!obj.IsMine) {
            return;
        }

        if (Input.GetMouseButtonDown(1) &&
            gameObject.transform.childCount > 1) {
            SeperateChips(1);
        }

    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == "Chips") {
            numberOfChip = gameObject.transform.childCount;
        }
    }

    void OnCollisionExit(Collision target)
    {
        if (target.gameObject.tag == "Chips" && isClicked) {
            int n = gameObject.transform.childCount;
            if (MoveToTarget(target.gameObject, n)) {
                objectManager.Delete(Object.Id);
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
        if (targetChips.value != value)
            return false;

        var sourceChips = GetComponent<Chips>();

        var targetCollider = target.GetComponent<BoxCollider>();
        var sourceCollider = GetComponent<BoxCollider>();

        Transform[] children = GetComponentsInChildren<Transform>();
        int loop = num;
        foreach (Transform child in children) {
            if (child.gameObject.tag == "Chip") {
                if (--loop < 0) break;

                targetChips.numberOfChip++;
                child.SetParent(target.transform);

                child.transform.localPosition = new Vector3(
                    0, 2 * (targetChips.numberOfChip - 1), 0
                );

                child.transform.localScale = Vector3.one; 
                child.transform.localRotation = Quaternion.identity;
                child.SetAsFirstSibling();
            }
        }
        sourceChips.numberOfChip -= num;
        RegulateCollider(sourceCollider, -num);
        RegulateCollider(targetCollider, num);
        return true;
    }

    public GameObject SeperateChips(int n)
    {
        GameObject tmp = Instantiate(newChips);
        InitObject(tmp, new Vector3(1.5f, 0, 0));
        tmp.GetComponent<Chips>().obj.PossessInfo =
            obj.PossessInfo;
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
                GameObject chip = Instantiate(chipPrefabs[value]);
                chip.transform.SetParent(gameObject.transform);
                numberOfChip++;
                chip.transform.localPosition =
                    new Vector3(0, 2 * (numberOfChip - 1), 0);
                chip.transform.localRotation = Quaternion.identity;
                chip.transform.SetAsFirstSibling();
            }
            var collider = GetComponent<BoxCollider>();
            RegulateCollider(collider, num - 1);
        }
        else {
            Transform[] children =
                GetComponentsInChildren<Transform>();
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
        objSnapshot.object_id = obj.object_id;
        objSnapshot.object_type = obj.object_type;
        objSnapshot.pos.Set(gameObject.transform.position);
        objSnapshot.height = numberOfChip;
        objSnapshot.value = value;

        objSnapshot.possess_info = obj.PossessInfo;
        return objSnapshot as ObjectSnapshot;
    }
    
    void SyncChips(ObjectSnapshot objSnapshot)
    {
        var syncInfo = objSnapshot as ChipsSnapshot;
        if(syncInfo.height != numberOfChip)
            SetChips(syncInfo.value, syncInfo.height - numberOfChip);
        gameObject.transform.position = syncInfo.pos.Get();
        value = syncInfo.value;
        numberOfChip = syncInfo.height;
    }
}
}

