using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInHold : MonoBehaviour
{
    private List<InteractiveObject> _itemsInHold =  new List<InteractiveObject>();

    public void AddObjectToItemList(InteractiveObject interactiveObject) => _itemsInHold.Add(interactiveObject);
    
    public bool FindItemInList(InteractiveObject neededItem)
    {
        if (_itemsInHold.Count == 0)
            return false;

        foreach (var item in _itemsInHold)
            if (item == neededItem)
                return true;

        return false;
    }
}
