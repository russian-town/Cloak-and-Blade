using System;
using UnityEditor.Sprites;
using UnityEngine;

public class Key : InteractiveObject
{
    [SerializeField] private InteractiveObjectView _view;
    [SerializeField] private AudioSource _source;
    [SerializeField] private GameObject _model;

    public event Action PickedUp;

    public override void Interact()
    {
        _source.Play();
        Player.ItemsInHold.AddObjectToItemList(this);
        _view.gameObject.SetActive(false);
        _model.gameObject.SetActive(false);
        PickedUp?.Invoke();
    }

    public override void Prepare()
    {
        if (CheckInteractionPossibility() && !Player.ItemsInHold.FindItemInList(this))
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

    protected override void Disable()
    {
        _view.InteractButton.onClick.RemoveListener(Interact);
    }
}
