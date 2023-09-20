using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    private List<Cell> _availableCells = new List<Cell>();

    public IReadOnlyList<Cell> AvailableCells => _availableCells;

    public void RefillAvailableCells(List<Cell> availableCells)
    {
        _availableCells.Clear();
        _availableCells = availableCells;
    }

    public void RefillAvailableCells(Cell currentCell)
    {
        _availableCells.Clear();
        _availableCells = new List<Cell> { currentCell.North, currentCell.North.North, currentCell.East, currentCell.East.East, currentCell.West, currentCell.West.West, currentCell.South, currentCell.South.South };
    }

    public void ClearAvailableCells() => _availableCells.Clear();

    public bool CanMoveToCell(Cell cell)
    {
        if (_availableCells.Contains(cell) && cell.Content.Type != CellContentType.Wall && cell != null)
            return true;

        return false;
    }
}
