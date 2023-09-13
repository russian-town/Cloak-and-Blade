using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortcullisKey : InteractiveObject
{
    public override void Interact()
    {
        if (!CheckInteractionPossibility())
        {
            print("Not in range");
            return;
        }
        else
        {
            print("Key acquired");
            Player.ItemsInHold.AddObjectToItemList(gameObject.GetComponent<PortcullisKey>());
            gameObject.SetActive(false);
        }
    }
}
