using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Player _player;
    private List<Cell> _tempCells = new List<Cell>();

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

        foreach (var icon in _icons)
            icon.Initialize();

        ShowInteravtiveButton();
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);

    public void ShowInteravtiveButton()
    {
        _move.interactable = true;
        _ability.interactable = true;
        _skip.interactable = true;

        foreach (var icon in _icons)
            icon.Interactable(true);
    }

    public void HideInteractiveButton()
    {
        _move.interactable = false;
        _ability.interactable = false;
        _skip.interactable = false;

        foreach (var icon in _icons)
            icon.Interactable(false);
    }

    public void SetPause(bool isPause)
    {
        if (isPause == true)
            Unsubscribe();
        else
            Subscribe();
    }

    private void OnMoveClick() => _player.PrepareMove();

    private void OnAbilityClick() => _player.PrepareAbility();

    private void OnSkipClick() => _player.PrepareSkip();
}
