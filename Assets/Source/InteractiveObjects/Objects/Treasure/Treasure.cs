using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Treasure : InteractiveObject
{
    [SerializeField] private InteractiveObjectView _view;
    private Animator _animator;
    private bool _treasureAccquired;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        _animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        _view.InteractButton.onClick.RemoveListener(Interact);
        _view.Hide();
        Open();
        _treasureAccquired = true;
    }

    public override void Prepare()
    {
        if (CheckInteractionPossibility())
        {
            _view.Show();
            _view.InteractButton.onClick.AddListener(Interact);
        }
        else if (_treasureAccquired)
        {
            _animator.SetTrigger(Constants.CloseParameter);
        }
    }

    private void Open()
    {
        _animator.SetTrigger(Constants.OpenParameter);
        Player.ItemsInHold.AddObjectToItemList(gameObject.GetComponent<Treasure>());
    }
}
