using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    private List<Cell> _availableNorthCells = new List<Cell>();
    private List<Cell> _availableSouthCells = new List<Cell>();
    private List<Cell> _availableWestCells = new List<Cell>();
    private List<Cell> _availableEasthCells = new List<Cell>();

    public IReadOnlyList<Cell> AvailableCells => _availableNorthCells;

    public void RefillAvailableCells(List<Cell> availableCells)
    {
        _availableNorthCells.Clear();
        _availableNorthCells.AddRange(availableCells);
    }

    public void RefillAvailableCells(Cell currentCell)
    {
        _availableNorthCells.Clear();
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

    public void ClearAvailableCells() => _availableNorthCells.Clear();

    public bool CanMoveToCell(ref Cell cell)
    {
        //if (_availableNorthCells.Contains(cell))
        //    return true;

        if(_availableNorthCells.Contains(cell))
        {
            foreach (var northCell in _availableNorthCells)
            {
                if(northCell.HasTrap)
                {
                    cell = northCell;
                    return true;
                }
            }

            return true;
        }
        else if(_availableSouthCells.Contains(cell))
        {
            foreach (var southCell in _availableSouthCells)
            {
                if (southCell.HasTrap)
                {
                    cell = southCell;
                    return true;
                }
            }
        }
        else if(_availableWestCells.Contains(cell))
        {
            foreach (var westCell in _availableWestCells)
            {
                if (westCell.HasTrap)
                {
                    cell = westCell;
                    return true;
                }
            }
        }
        else if(_availableEasthCells.Contains(cell))
        {
            foreach (var eastCell in _availableEasthCells)
            {
                if (eastCell.HasTrap)
                {
                    cell = eastCell;
                    return true;
                }
            }
        }

        return false;
    }

    private void AddCell(Cell cell)
    {
        if (cell == null || cell.Content.Type == CellContentType.Wall)
            return;

        _availableNorthCells.Add(cell);
    }
}
