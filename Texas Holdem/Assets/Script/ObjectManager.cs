using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
public class ObjectManager {

    private Dictionary<uint, GameObject> dictionary_;
    uint index_;

    public GameObject Find(uint id)
    {
        if (dictionary_.ContainsKey(id))
            return dictionary_[id];
        else
            return null;
    }

    public uint Add(GameObject obj)
    {
        dictionary_.Add(index_++, obj);
        return index_;
    }

    public bool Add(uint id, GameObject obj)
    {
        if (dictionary_.ContainsKey(id)) return false;
        dictionary_.Add(id, obj);
        return true;
    }

    public bool Delete(uint id)
    {
        if (!dictionary_.ContainsKey(id)) return false;
        dictionary_.Remove(id);
        return true;
    }
}
}
