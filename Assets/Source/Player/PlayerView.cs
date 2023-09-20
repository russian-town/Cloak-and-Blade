using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Button _move;
    [SerializeField] private Button _ability;
    [SerializeField] private Button _skip;
    
    private Player _player;
    private List<Cell> _tempCells = new List<Cell>();

    private void OnDestroy()
    {
        Hide();
    }

    public void Initialize(Player player)
    {
        _player = player;
        Show();
    }

    public void Show()
    {
        _move.onClick.AddListener(OnMoveClick);
        _ability.onClick.AddListener(OnAbilityClick);
        _skip.onClick.AddListener(OnSkipClick);
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        _move.onClick.RemoveListener(OnMoveClick);
        _ability.onClick.RemoveListener(OnAbilityClick);
        _skip.onClick.RemoveListener(OnSkipClick);
        gameObject.SetActive(false);
    }

    public void ShowAvailableCells(List<Cell> cells)
    {
        HideAvailableCells();
        _tempCells = cells;

        foreach (var cell in _tempCells)
            if (cell.Content.Type != CellContentType.Wall || cell.IsOccupied)
                cell.View.Show();
    }

    public void HideAvailableCells()
    {
        if (_tempCells.Count > 0)
            foreach (var cell in _tempCells)
                cell.View.Hide();

        _tempCells.Clear();
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
