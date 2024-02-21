using System;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : InteractiveObject, ICompassTarget
{
    [SerializeField] private InteractiveObjectView _view;
    [SerializeField] private Key _key;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private Image _unLockedImage;
    [SerializeField] private AudioSource _chestSource;
    [SerializeField] private AudioSource _lockSource;
    [SerializeField] private AudioSource _closeSource;
    [SerializeField] private ParticleSystem _particle;

    private Animator _animator;
    private bool _treasureAccquired;
    private bool _closed = true;

    public event Action Opened;
    public event Action Disabled;

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
            return;
        }

        _view.InteractButton.onClick.RemoveListener(Interact);
        _view.Hide();
        _lockSource.Play();
        _chestSource.Play();
        _particle.Stop();
        Open();
        _closed = false;
        _treasureAccquired = true;
        Disabled?.Invoke();
    }

    public override void Prepare()
    {
        if (CheckInteractionPossibility() && !Player.ItemsInHold.FindItemInList(this))
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

            if (!_closed)
                _closeSource.Play();

            _closed = true;
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
        Player.ItemsInHold.AddObjectToItemList(this);
        Opened?.Invoke();
    }

    protected override void Disable()
    {
        _view.InteractButton.onClick.RemoveListener(Interact);
    }
}
