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
    [SerializeField] private Button _abilityExecuteButton;
    [SerializeField] private Button _leftRotationCameraButton;
    [SerializeField] private Button _rightRotationCameraButton;
    [SerializeField] private Button _perspectiveCameraButton;
    [SerializeField] private List<Icon> _icons = new List<Icon>();

    private Player _player;
    private List<Cell> _tempCells = new List<Cell>();

    public event Action PauseButtonClicked;
    public event Action AbilityExecuteButtonClicked;

    public void Subscribe()
    {
        _move.onClick.AddListener(OnMoveClick);
        _ability.onClick.AddListener(OnAbilityClick);
        _skip.onClick.AddListener(OnSkipClick);
        _pause.onClick.AddListener(() => PauseButtonClicked?.Invoke());
        _abilityExecuteButton.onClick.AddListener(() => AbilityExecuteButtonClicked?.Invoke());
    }

    public void Unsubscribe()
    {
        _move.onClick.RemoveListener(OnMoveClick);
        _ability.onClick.RemoveListener(OnAbilityClick);
        _skip.onClick.RemoveListener(OnSkipClick);
        _pause.onClick.RemoveListener(() => PauseButtonClicked?.Invoke());
        _abilityExecuteButton.onClick.RemoveListener(() => AbilityExecuteButtonClicked?.Invoke());
    }

    public void Initialize(Player player)
    {
        _player = player;
        _abilityExecuteButton.gameObject.SetActive(false);

        foreach (var icon in _icons)
            icon.Initialize();

        ShowInteravtiveButton();
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);

    public void ShowAbilityExecuteButton() => _abilityExecuteButton.gameObject.SetActive(true);

    public void HideAbilityExecuteButton() => _abilityExecuteButton.gameObject.SetActive(false);

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

    public void ShowAvailableCells(List<Cell> cells)
    {
        HideAvailableCells();
        _tempCells = cells;

        if (_tempCells.Count == 0)
            return;

        foreach (var cell in _tempCells)
        {
            if (cell.Content.Type != CellContentType.Wall)
                cell.View.Show();
        }
    }

    public void HideAvailableCells()
    {
        if (_tempCells.Count == 0)
            return;

        foreach (var cell in _tempCells)
        {
            cell.View.Hide();
        }

        _tempCells.Clear();
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
