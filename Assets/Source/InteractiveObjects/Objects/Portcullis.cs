using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portcullis : InteractiveObject
{
    [SerializeField] private Cell[] _wallCells;
    [SerializeField] private PortcullisKey _key;

    public override void Interact()
    {
        TryOpen();
    }

    public void TryOpen()
    {
        if (!CheckInteractionPossibility())
        {
            print("Not in range");
            return;
        }

        foreach(var item in Player.ItemsInHold.ItemList)
        {
            if (item == _key)
            {
                print("Opened succecfully");

                foreach (var cell in _wallCells)
                    cell.Content.BecomeEmpty();

                gameObject.SetActive(false);
                break;
            }
            else
            {
                print("Needs key");
            }
        }
    }
}
