using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour, IPauseHandler
{
    [SerializeField] private Button _move;
    [SerializeField] private Button _ability;
    [SerializeField] private Button _skip;
    
    private Player _player;
    private List<Cell> _tempCells = new List<Cell>();

    public void Subscribe()
    {
        _move.onClick.AddListener(OnMoveClick);
        _ability.onClick.AddListener(OnAbilityClick);
        _skip.onClick.AddListener(OnSkipClick);
    }

    public void Unsubscribe()
    {
        _move.onClick.RemoveListener(OnMoveClick);
        _ability.onClick.RemoveListener(OnAbilityClick);
        _skip.onClick.RemoveListener(OnSkipClick);
    }

    public void Initialize(Player player)
    {
        _player = player;
        Show();
    }

    public void Show()
    {
        if (gameObject.activeSelf == true)
            return;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (gameObject.activeSelf == false)
            return;

        gameObject.SetActive(false);
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

    private void OnMoveClick()
    {
        _player.PrepareMove();
    }
    
    private void OnAbilityClick()
    {
        _player.PrepareAbility();
    }
    
    private void OnSkipClick() => _player.PrepareSkip();
}
