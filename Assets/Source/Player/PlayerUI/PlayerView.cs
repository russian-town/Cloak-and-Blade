using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(CanvasGroup))]
public class PlayerView : MonoBehaviour, IPauseHandler
{
    [SerializeField] private Button _move;
    [SerializeField] private Button _ability;
    [SerializeField] private Button _skip;
    [SerializeField] private Button _pause;
    [SerializeField] private Button _leftRotationCameraButton;
    [SerializeField] private Button _rightRotationCameraButton;
    [SerializeField] private Button _perspectiveCameraButton;
    [SerializeField] private List<Icon> _icons = new List<Icon>();
    [SerializeField] private Icon _abilityIcon;

    private Player _player;
    private CanvasGroup _canvasGroup;
    private bool _canSwitchInteractable = true;
    private bool _isInteractable;

    public event Action PauseButtonClicked;

    public void Subscribe()
    {
        _move.onClick.AddListener(OnMoveClick);
        _ability.onClick.AddListener(OnAbilityClick);
        _skip.onClick.AddListener(OnSkipClick);
        _pause.onClick.AddListener(() => PauseButtonClicked?.Invoke());
    }

    public void Unsubscribe()
    {
        _move.onClick.RemoveListener(OnMoveClick);
        _ability.onClick.RemoveListener(OnAbilityClick);
        _skip.onClick.RemoveListener(OnSkipClick);
        _pause.onClick.RemoveListener(() => PauseButtonClicked?.Invoke());
    }

    public void Initialize(Player player)
    {
        _player = player;
        _canvasGroup = GetComponent<CanvasGroup>();

        foreach (var icon in _icons)
            icon.Initialize();

        _abilityIcon.ChangeSprite(_player.AbilityIcon);
        ShowInteravtiveButton();
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    public void ShowInteravtiveButton()
    {
        _isInteractable = true;
        _move.interactable = true;
        _skip.interactable = true;

        if (_canSwitchInteractable == true)
            _ability.interactable = true;

        foreach (var icon in _icons)
        {
            if (icon == _abilityIcon && _canSwitchInteractable == false)
                continue;

            icon.Interactable(true);
        }
    }

    public void HideInteractiveButton()
    {
        _isInteractable = false;
        _move.interactable = false;
        _skip.interactable = false;

        if (_canSwitchInteractable == true)
            _ability.interactable = false;

        foreach (var icon in _icons)
        {
            if (icon == _abilityIcon && _canSwitchInteractable == false)
                continue;

            icon.Interactable(false);
        }
    }

    public void SetPause(bool isPause)
    {
        if (isPause == true)
            Unsubscribe();
        else
            Subscribe();
    }

    public void Cansel() => _canSwitchInteractable = true;

    public void EnableAbilityButton()
    {
        _ability.interactable = _isInteractable;
        _abilityIcon.Interactable(_isInteractable);
    }

    public void DisableAbilityButton()
    {
        _canSwitchInteractable = false;
        _ability.interactable = false;
        _abilityIcon.Interactable(false);
    }

    private void OnMoveClick() => _player.PrepareMove();

    private void OnAbilityClick() => _player.TryPrepareAbility();

    private void OnSkipClick() => _player.PrepareSkip();
}
