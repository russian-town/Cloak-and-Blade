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
    [SerializeField] private Icon _moveIcon;
    [SerializeField] private Sprite _rewardedImage;
    [SerializeField] private AudioSource _tickTockSound;

    private Player _player;
    private CommandExecuter _commandExecuter;
    private CanvasGroup _canvasGroup;
    private bool _canSwitchAbilityInteractable = true;
    private bool _canSwitchMoveInteractable = true;
    private bool _isAbilityInteractable;
    private bool _isMoveInteractable;

    public event Action PauseButtonClicked;

    private void OnDisable()
    {
        _player.AbilityUsed -= OnAbilityUsed;
        _commandExecuter.AbilityReseted -= OnAbilityReseted;
    }

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

    public void Initialize(Player player, CommandExecuter commandExecuter)
    {
        _player = player;
        _commandExecuter = commandExecuter;
        _player.AbilityUsed += OnAbilityUsed;
        _commandExecuter.AbilityReseted += OnAbilityReseted;
        _canvasGroup = GetComponent<CanvasGroup>();

        foreach (var icon in _icons)
            icon.Initialize();

        _abilityIcon.ChangeSprite(_player.AbilityIcon);
        ShowInteravtiveButton();
    }

    public void HideMoveButton()
    {
        _canSwitchMoveInteractable = false;
        _move.interactable = false;
        _moveIcon.Interactable(false);
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void ShowInteravtiveButton()
    {
        _isAbilityInteractable = true;
        _isMoveInteractable = true;
        _skip.interactable = true;

        if (_canSwitchMoveInteractable == true)
            _move.interactable = true;

        if (_canSwitchAbilityInteractable == true)
            _ability.interactable = true;

        foreach (var icon in _icons)
        {
            if (icon == _abilityIcon && _canSwitchAbilityInteractable == false)
                continue;

            if (icon == _moveIcon && _canSwitchMoveInteractable == false)
                continue;

            icon.Interactable(true);
        }
    }

    public void HideInteractiveButton()
    {
        _isAbilityInteractable = false;
        _skip.interactable = false;

        if (_canSwitchMoveInteractable == true)
            _move.interactable = false;

        if (_canSwitchAbilityInteractable == true)
            _ability.interactable = false;

        foreach (var icon in _icons)
        {
            if (icon == _abilityIcon && _canSwitchAbilityInteractable == false)
                continue;

            if (icon == _moveIcon && _canSwitchMoveInteractable == false)
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

    public void Cancel()
    {
        _canSwitchAbilityInteractable = true;
        _canSwitchMoveInteractable = true;
    }

    public void EnableAbilityButton()
    {
        _ability.interactable = _isAbilityInteractable;
        _abilityIcon.Interactable(_isAbilityInteractable);
    }

    public void ResetAbilityIcon()
    {
        _abilityIcon.ChangeSprite(_player.AbilityIcon);
        _abilityIcon.StopShaking();
    }

    public void DisableAbilityButton()
    {
        _canSwitchAbilityInteractable = false;
        _ability.interactable = false;
        _abilityIcon.Interactable(false);
    }

    private void OnAbilityUsed()
    {
        _abilityIcon.ChangeSprite(_rewardedImage);
        _abilityIcon.PlayShakeEffect();
    } 

    private void OnAbilityReseted() => ResetAbilityIcon();

    private void OnMoveClick() => _player.PrepareMove();

    private void OnAbilityClick() => _player.TryPrepareAbility();

    private void OnSkipClick()
    {
        _player.PrepareSkip();
        _tickTockSound.Play();
    } 
}
