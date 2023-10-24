using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    private List<Cell> _availableNorthCells = new List<Cell>();
    private List<Cell> _availableSouthCells = new List<Cell>();
    private List<Cell> _availableWestCells = new List<Cell>();
    private List<Cell> _availableEasthCells = new List<Cell>();
    private List<Cell> _availableCells = new List<Cell>();
    private List<Cell> _tempCells = new List<Cell>();

    public void RefillAvailableCells(List<Cell> availableCells)
    {
        _availableNorthCells.Clear();
        _availableNorthCells.AddRange(availableCells);
    }

    public void RefillAvailableCells(Cell currentCell)
    {
        _availableNorthCells.Clear();
        _availableSouthCells.Clear();
        _availableEasthCells.Clear();
        _availableWestCells.Clear();
        _availableCells.Clear();

        AddCell(currentCell.North, _availableNorthCells);
        AddCell(currentCell.North, _availableCells);
        AddCell(currentCell.South, _availableSouthCells);
        AddCell(currentCell.South, _availableCells);
        AddCell(currentCell.West, _availableWestCells);
        AddCell(currentCell.West, _availableCells);
        AddCell(currentCell.East, _availableEasthCells);
        AddCell(currentCell.East, _availableCells);

        if (currentCell.North != null && currentCell.North.Content.Type != CellContentType.Wall)
        {
            AddCell(currentCell.North.North, _availableNorthCells);
            AddCell(currentCell.North.North, _availableCells);
        }
        if (currentCell.South != null && currentCell.South.Content.Type != CellContentType.Wall)
        {
            AddCell(currentCell.South.South, _availableSouthCells);
            AddCell(currentCell.South.South, _availableCells);
        }
        if (currentCell.West != null && currentCell.West.Content.Type != CellContentType.Wall)
        {
            AddCell(currentCell.West.West, _availableWestCells);
            AddCell(currentCell.West.West, _availableCells);
        }
        if (currentCell.East != null && currentCell.East.Content.Type != CellContentType.Wall)
        {
            AddCell(currentCell.East.East, _availableEasthCells);
            AddCell(currentCell.East.East, _availableCells);
        }
    }

    public bool CanMoveToCell(ref Cell cell)
    {
        Cell targetNorthCell = FindCellHasTrap(_availableNorthCells, cell);
        Cell targetSouthCell = FindCellHasTrap(_availableSouthCells, cell);
        Cell targetEastCell = FindCellHasTrap(_availableEasthCells, cell);
        Cell targetWestCell = FindCellHasTrap(_availableWestCells, cell);

        if (targetNorthCell != null)
        {
            cell = targetNorthCell;
            return true;
        }
        else if(targetSouthCell != null)
        {
            cell = targetSouthCell;
            return true;
        }
        else if(targetEastCell != null)
        {
            cell = targetEastCell;
            return true;
        }
        else if(targetWestCell != null)
        {
            cell = targetWestCell;
            return true;
        }

        return false;
    }

    public void ShowAvailableCells()
    {
        HideAvailableCells();

        if (_availableCells.Count == 0)
            return;

        foreach (var cell in _availableCells)
        {
            if (cell.Content.Type != CellContentType.Wall)
                cell.View.Show();
        }

        _tempCells.AddRange(_availableCells);
    }

    public void HideAvailableCells()
    {
        if(_tempCells.Count == 0) 
            return;

        foreach (var cell in _tempCells)
        {
            cell.View.Hide();
        }

        _tempCells.Clear();
    }

    private Cell FindCellHasTrap(List<Cell> cells, Cell targetCell)
    {
        if (cells.Contains(targetCell))
        {
            foreach (var cell in cells)
            {
                if (cell.HasTrap)
                {
                    if(cells.IndexOf(targetCell) < cells.IndexOf(cell))
                    {
                        return targetCell;
                    }

                    return cell;
                }
            }

            return targetCell;
        }

        return null;
    }

    private void AddCell(Cell cell, List<Cell> cells)
    {
        if (cell == null || cell.Content.Type == CellContentType.Wall)
            return;

        cells.Add(cell);
    }
}
