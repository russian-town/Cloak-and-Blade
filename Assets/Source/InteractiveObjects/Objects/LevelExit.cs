using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelExit : InteractiveObject, ILevelFinisher
{
    [SerializeField] private InteractiveObjectView _view;
    [SerializeField] private Treasure _treasure;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private Image _unLockedImage;

    private bool _unLocked = false;

    public event UnityAction LevelPassed;

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
            Disable();
    }

    public override void Prepare()
    {
        if (_unLocked)
            return;

        if (CheckInteractionPossibility())
        {
            _view.Show();

            if (Player.ItemsInHold.FindItemInList(_treasure))
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

    public bool TryOpen()
    {
        if (_unLocked)
            return false;

        if (Player.ItemsInHold.FindItemInList(_treasure))
        {
            print("level passed");
            LevelPassed?.Invoke();
            _unLocked = true;
            return true;
        }
        else
        {
            print("level not passed");
            LevelPassed?.Invoke();
        }

        return false;
    }

    protected override void Disable()
    {
        _view.InteractButton.onClick.RemoveListener(Interact);
    }
}
