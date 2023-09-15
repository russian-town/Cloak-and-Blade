using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portcullis : InteractiveObject
{
    [SerializeField] private Cell[] _wallCells;
    [SerializeField] private PortcullisKey _key;
    [SerializeField] private InteractiveObjectView _view;

    public override void Interact()
    {
        TryOpen();
    }

    public override void Prepare()
    {
        if (CheckInteractionPossibility())
        {
            _view.Show();
            _view.InteractButton.onClick.AddListener(Interact);
        }
        else if (_view.isActiveAndEnabled)
        {
            _view.InteractButton.onClick.RemoveListener(Interact);
            _view.Hide();
        }
    }

    public void TryOpen()
    {
        if (Player.ItemsInHold.FindItemInList(_key))
        {
            print("Opened succecfully");

            foreach (var cell in _wallCells)
                cell.Content.BecomeEmpty();

            gameObject.SetActive(false);
        }
        else
        {
            print("Find key firts");
        }
    }
}
