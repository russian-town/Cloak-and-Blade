using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class PlayerView : MonoBehaviour, IPauseHandler
{
    [SerializeField] private Button _move;
    [SerializeField] private Button _ability;
    [SerializeField] private Button _skip;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private List<Icon> _icons = new ();
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
    private ILevelFinisher _levelFinisher;
    private Pause _pause;

    public event Action PauseButtonClicked;

    private void OnDisable()
    {
        _player.AbilityUsed -= OnAbilityUsed;
        _commandExecuter.AbilityReseted -= OnAbilityReseted;
        _levelFinisher.LevelPassed -= OnLevelPassed;
        _levelFinisher.LevelFailed -= OnLevelFailed;
    }

    public void Subscribe()
    {
        _move.onClick.AddListener(OnMoveClick);
        _ability.onClick.AddListener(OnAbilityClick);
        _skip.onClick.AddListener(OnSkipClick);
        _pauseButton.onClick.AddListener(() => PauseButtonClicked?.Invoke());
    }

    public void Unsubscribe()
    {
        _move.onClick.RemoveListener(OnMoveClick);
        _ability.onClick.RemoveListener(OnAbilityClick);
        _skip.onClick.RemoveListener(OnSkipClick);
        _pauseButton.onClick.RemoveListener(() => PauseButtonClicked?.Invoke());
    }

    public void Initialize(
        Player player,
        CommandExecuter commandExecuter,
        ILevelFinisher levelFinisher,
        Pause pause)
    {
        _player = player;
        _commandExecuter = commandExecuter;
        _levelFinisher = levelFinisher;
        _player.AbilityUsed += OnAbilityUsed;
        _commandExecuter.AbilityReseted += OnAbilityReseted;
        _levelFinisher.LevelPassed += OnLevelPassed;
        _levelFinisher.LevelFailed += OnLevelFailed;
        _pause.Enabled += OnEnabled;
        _pause.Disabled += OnDisabled;
        _canvasGroup = GetComponent<CanvasGroup>();

        foreach (var icon in _icons)
            icon.Initialize();

        _abilityIcon.ChangeSprite(_player.AbilityIcon);
        ShowInteractiveButton();
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

    public void ShowInteractiveButton()
    {
        _isAbilityInteractable = true;
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

    public void Unpause()
        => Subscribe();

    public void Pause()
        => Unsubscribe();

    private void OnLevelPassed()
        => Hide();

    private void OnLevelFailed()
        => Hide();

    private void OnAbilityUsed()
    {
        _abilityIcon.ChangeSprite(_rewardedImage);
        _abilityIcon.PlayShakeEffect();
    }

    private void OnAbilityReseted()
        => ResetAbilityIcon();

    private void OnMoveClick()
        => _player.PrepareMove();

    private void OnAbilityClick()
        => _player.TryPrepareAbility();

    private void OnSkipClick()
    {
        _player.PrepareSkip();
        _tickTockSound.Play();
    }

    private void OnEnabled()
        => Hide();

    private void OnDisabled()
        => Show();
}
