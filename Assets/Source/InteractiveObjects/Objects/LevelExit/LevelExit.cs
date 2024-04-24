using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class LevelExit : InteractiveObject
{
    [SerializeField] private InteractiveObjectView _view;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private Image _unLockedImage;

    private bool _unLocked = false;

    public event Action ExitOpened;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        _lockedImage.gameObject.SetActive(false);
        _unLockedImage.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        if (_unLocked)
            return;

        if (TryOpen())
        {
            _view.gameObject.SetActive(false);
            _unLocked = true;
            Disable();
        }
    }

    public override void Prepare()
    {
        if (_unLocked)
            return;

        if (CheckInteractionPossibility())
        {
            _view.Show();

            if (RequiredItemFound())
            {
                _lockedImage.gameObject.SetActive(false);
                _unLockedImage.gameObject.SetActive(true);
            }
            else
            {
                _lockedImage.gameObject.SetActive(true);
            }

            _view.InteractButton.onClick.AddListener(Interact);
        }
        else if (_view.isActiveAndEnabled)
        {
            _view.InteractButton.onClick.RemoveListener(Interact);
            _view.Hide();
        }
    }

    public abstract bool TryOpen();

    public abstract bool RequiredItemFound();

    protected void InvokeExitOpened()
        => ExitOpened?.Invoke();

    protected override void Disable()
        => _view.InteractButton.onClick.RemoveListener(Interact);
}
