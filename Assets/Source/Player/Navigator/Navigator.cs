using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    private List<Cell> _availableCells = new List<Cell>();

    public IReadOnlyList<Cell> AvailableCells => _availableCells;

    public void RefillAvailableCells(List<Cell> availableCells)
    {
        _availableCells.Clear();
        _availableCells.AddRange(availableCells);
    }

    public void RefillAvailableCells(Cell currentCell)
    {
        _availableCells.Clear();
        AddCell(currentCell.North);
        AddCell(currentCell.South);
        AddCell(currentCell.West);
        AddCell(currentCell.East);

        if (currentCell.North != null && currentCell.North.Content.Type != CellContentType.Wall)
            AddCell(currentCell.North.North);
        if (currentCell.South != null && currentCell.South.Content.Type != CellContentType.Wall)
            AddCell(currentCell.South.South);
        if (currentCell.West != null && currentCell.West.Content.Type != CellContentType.Wall)
            AddCell(currentCell.West.West);
        if (currentCell.East != null && currentCell.East.Content.Type != CellContentType.Wall)
            AddCell(currentCell.East.East);
    }

    public void ClearAvailableCells() => _availableCells.Clear();

    public bool CanMoveToCell(Cell cell)
    {
        if (_availableCells.Contains(cell))
            return true;

        return false;
    }

    private void AddCell(Cell cell)
    {
        if (cell == null || cell.Content.Type == CellContentType.Wall)
            return;

        _availableCells.Add(cell);
    }
}
