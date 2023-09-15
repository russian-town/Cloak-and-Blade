using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortcullisKey : InteractiveObject
{
    [SerializeField] private InteractiveObjectView _view;

    public override void Interact()
    {
        print("Key acquired");
        Player.ItemsInHold.AddObjectToItemList(gameObject.GetComponent<PortcullisKey>());
        gameObject.SetActive(false);
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
}
