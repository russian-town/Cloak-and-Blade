using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : InteractiveObject
{
    [SerializeField] private GameObject _linza;
    [SerializeField] private InteractiveObjectView _view;

    public bool IsActive { get; private set; }

    public override void Interact()
    {
        if (IsActive)
            return;

        IsActive = true;
        _linza.SetActive(true);
    }

    public override void Prepare()
    {
        if (CheckInteractionPossibility())
            _view.Show();
    }

    protected override void Disable()
    {
        //throw new System.NotImplementedException();
    }
}
