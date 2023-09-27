using UnityEngine;
using UnityEngine.UI;

public class LevelExit : InteractiveObject
{
    [SerializeField] private InteractiveObjectView _view;
    [SerializeField] private Treasure _treasure;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private Image _unLockedImage;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        _lockedImage.gameObject.SetActive(false);
        _unLockedImage.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        TryOpen();
    }

    public override void Prepare()
    {
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

    public void TryOpen()
    {
        if (Player.ItemsInHold.FindItemInList(_treasure))
            print("level passed");
        else
            print("level not passed");
    }

    protected override void Disable()
    {
        _view.InteractButton.onClick.RemoveListener(Interact);
    }
}
