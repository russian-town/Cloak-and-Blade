using UnityEngine;
using UnityEngine.UI;

public class Treasure : InteractiveObject
{
    [SerializeField] private InteractiveObjectView _view;
    [SerializeField] private Key _key;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private Image _unLockedImage;
    private Animator _animator;
    private bool _treasureAccquired;

    private void OnDisable()
    {
        _view.InteractButton.onClick.RemoveListener(Interact);
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        _animator = GetComponent<Animator>();
        _lockedImage.gameObject.SetActive(false);
        _unLockedImage.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        if (!Player.ItemsInHold.FindItemInList(_key))
        {
            print("Requires key");
            return;
        }

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

            if (Player.ItemsInHold.FindItemInList(_key))
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
        else if (_treasureAccquired)
        {
            _animator.SetTrigger(Constants.CloseParameter);
        }
        else if (_view.isActiveAndEnabled)
        {
            _view.InteractButton.onClick.RemoveListener(Interact);
            _view.Hide();
        }
    }

    private void Open()
    {
        _animator.SetTrigger(Constants.OpenParameter);
        Player.ItemsInHold.AddObjectToItemList(gameObject.GetComponent<Treasure>());
    }
}
