using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    private List<Cell> _availableCells = new List<Cell>();

    public IReadOnlyList<Cell> AvailableCells => _availableCells;

    public void RefillAvailableCells(List<Cell> availableCells)
    {
        Debug.Log(availableCells.Count);
        _availableCells.Clear();
        _availableCells.AddRange(availableCells);
        Debug.Log(_availableCells.Count);
    }

    public void RefillAvailableCells(Cell currentCell)
    {
        Debug.Log("Refil2");
        _availableCells.Clear();
        AddCell(currentCell.North);
        AddCell(currentCell.South);
        AddCell(currentCell.West);
        AddCell(currentCell.East);
        AddCell(currentCell.North.North);
        AddCell(currentCell.South.South);
        AddCell(currentCell.West.West);
        AddCell(currentCell.East.East);
    }

    public void ClearAvailableCells() => _availableCells.Clear();

    public bool CanMoveToCell(Cell cell)
    {
        if (_availableCells.Contains(cell) && cell.Content.Type != CellContentType.Wall && cell != null)
            return true;

        return false;
    }

    private void AddCell(Cell cell)
    {
        if (cell == null)
            return;

        _availableCells.Add(cell);
    }
}
