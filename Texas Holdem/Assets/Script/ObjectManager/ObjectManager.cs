using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
public class ObjectManager {

    private Dictionary<int, Object> dictionary_;
    private int index_;

    public int Count {
        get { 
            return dictionary_.Count;
        }
    }

    public ObjectManager()
    {
        dictionary_ = new Dictionary<int, Object>();
    }

    public Object Find(int id)
    {
        if (dictionary_.ContainsKey(id))
            return dictionary_[id];
        else
            return null;
    }

    public int Add(Object obj)
    {
        dictionary_.Add(obj.object_id, obj);
        return index_;
    }

    public bool Delete(int id)
    {
        if (!dictionary_.ContainsKey(id)) return false;
        dictionary_.Remove(id);
        return true;
    }

    public ObjectSnapshot[] CreateObjectsSnapshot()
    {
        ObjectSnapshot[] objects_snapshot = 
            new ObjectSnapshot[dictionary_.Count];
        int i = 0;

        foreach (var kv in dictionary_) {
            if (kv.Value.snapshot_handler == null) continue;
            objects_snapshot[i] = kv.Value.snapshot_producer();
        }

        return objects_snapshot;
    }
}
}
