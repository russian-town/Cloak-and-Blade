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
        ShowInteravtiveButton();
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);

    public void ShowInteravtiveButton()
    {
        _move.interactable = true;
        _ability.interactable = true;
        _skip.interactable = true;
        _leftRotationCameraButton.interactable = true;
        _rightRotationCameraButton.interactable = true;
        _perspectiveCameraButton.interactable = true;
    }

    public void HideInteractiveButton()
    {
        _move.interactable = false;
        _ability.interactable = false;
        _skip.interactable = false;
        _leftRotationCameraButton.interactable = false;
        _rightRotationCameraButton.interactable = false;
        _perspectiveCameraButton.interactable = false;
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
