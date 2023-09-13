using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInHold : MonoBehaviour
{
    private List<InteractiveObject> _itemsInHold =  new List<InteractiveObject>();

    public IReadOnlyList<InteractiveObject> ItemList => _itemsInHold;

    public void AddObjectToItemList(InteractiveObject interactiveObject) => _itemsInHold.Add(interactiveObject);
}
