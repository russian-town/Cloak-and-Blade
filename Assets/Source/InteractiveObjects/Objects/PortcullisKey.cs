using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortcullisKey : InteractiveObject
{
    public override void Interact()
    {
        print("Key acquired");
        Player.ItemsInHold.AddObjectToItemList(gameObject.GetComponent<PortcullisKey>());
        gameObject.SetActive(false);
    }
}
